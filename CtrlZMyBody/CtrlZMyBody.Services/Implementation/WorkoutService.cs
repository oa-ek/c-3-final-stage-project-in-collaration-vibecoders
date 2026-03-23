using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutPlanRepository _planRepo;
        private readonly IUserWorkoutSessionRepository _sessionRepo;

        public WorkoutService(IWorkoutPlanRepository planRepo,
            IUserWorkoutSessionRepository sessionRepo)
        {
            _planRepo = planRepo;
            _sessionRepo = sessionRepo;
        }

        public async Task<WorkoutPlan?> GetPlanForUserAsync(int injuryTypeId, int difficultyLevelId) =>
            await _planRepo.GetByInjuryAndDifficultyAsync(injuryTypeId, difficultyLevelId);

        public async Task<WorkoutPlanDay?> GetDayAsync(int planDayId) =>
            await _planRepo.GetDayWithExercisesAsync(planDayId);

        public async Task<UserWorkoutSession> StartSessionAsync(int userId, int planDayId)
        {
            var day = await _planRepo.GetDayWithExercisesAsync(planDayId)
                ?? throw new Exception("День плану не знайдено.");

            var session = new UserWorkoutSession
            {
                UserId = userId,
                PlanDayId = planDayId,
                SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
                TotalExercises = day.Exercises.Count,
                StartedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            return await _sessionRepo.AddAsync(session);
        }

        public async Task<UserWorkoutSession> CompleteExerciseAsync(int sessionId, int exerciseId,
            int sets, int reps, int durationSec)
        {
            var session = await _sessionRepo.GetSessionWithLogsAsync(sessionId)
                ?? throw new Exception("Сесію не знайдено.");

            // Перевіряємо чи вправа вже залогована
            var existingLog = session.ExerciseLogs.FirstOrDefault(l => l.ExerciseId == exerciseId);
            if (existingLog != null)
            {
                existingLog.IsCompleted = true;
                existingLog.ActualSets = sets;
                existingLog.ActualReps = reps;
                existingLog.ActualDurationSec = durationSec;
                existingLog.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                session.ExerciseLogs.Add(new UserExerciseLog
                {
                    SessionId = sessionId,
                    ExerciseId = exerciseId,
                    IsCompleted = true,
                    ActualSets = sets,
                    ActualReps = reps,
                    ActualDurationSec = durationSec,
                    CompletedAt = DateTime.UtcNow
                });
            }

            session.CompletedExercises = session.ExerciseLogs.Count(l => l.IsCompleted);
            await _sessionRepo.UpdateAsync(session);
            return session;
        }

        public async Task<UserWorkoutSession> FinishSessionAsync(int sessionId)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId)
                ?? throw new Exception("Сесію не знайдено.");

            session.IsCompleted = true;
            session.FinishedAt = DateTime.UtcNow;
            await _sessionRepo.UpdateAsync(session);
            return session;
        }

        public async Task<IEnumerable<UserWorkoutSession>> GetUserHistoryAsync(int userId) =>
            await _sessionRepo.GetByUserIdAsync(userId);

        public async Task<UserWorkoutSession?> GetSessionAsync(int sessionId) =>
            await _sessionRepo.GetSessionWithLogsAsync(sessionId);
    }
}