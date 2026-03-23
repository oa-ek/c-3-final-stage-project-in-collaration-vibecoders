using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class UserWorkoutSessionRepository : Repository<UserWorkoutSession>, IUserWorkoutSessionRepository
    {
        public UserWorkoutSessionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<UserWorkoutSession>> GetByUserIdAsync(int userId) =>
            await _dbSet
                .Include(s => s.PlanDay)
                    .ThenInclude(d => d!.Plan)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SessionDate)
                .ToListAsync();

        public async Task<UserWorkoutSession?> GetSessionWithLogsAsync(int sessionId) =>
            await _dbSet
                .Include(s => s.ExerciseLogs)
                    .ThenInclude(l => l.Exercise)
                .Include(s => s.PlanDay)
                    .ThenInclude(d => d!.Exercises)
                        .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        public async Task<int> GetCompletedCountAsync(int userId) =>
            await _dbSet.CountAsync(s => s.UserId == userId && s.IsCompleted);

        public async Task<IEnumerable<UserWorkoutSession>> GetRecentAsync(int userId, int count = 7) =>
            await _dbSet
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SessionDate)
                .Take(count)
                .ToListAsync();
    }
}