using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<WorkoutPlan?> GetPlanForUserAsync(int injuryTypeId, int difficultyLevelId);
        Task<WorkoutPlanDay?> GetDayAsync(int planDayId);
        Task<UserWorkoutSession> StartSessionAsync(int userId, int planDayId);
        Task<UserWorkoutSession> CompleteExerciseAsync(int sessionId, int exerciseId,
            int sets, int reps, int durationSec);
        Task<UserWorkoutSession> FinishSessionAsync(int sessionId);
        Task<IEnumerable<UserWorkoutSession>> GetUserHistoryAsync(int userId);
        Task<UserWorkoutSession?> GetSessionAsync(int sessionId);
    }
}