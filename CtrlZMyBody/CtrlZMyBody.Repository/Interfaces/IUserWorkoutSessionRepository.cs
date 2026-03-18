using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IUserWorkoutSessionRepository : IRepository<UserWorkoutSession>
    {
        Task<IEnumerable<UserWorkoutSession>> GetByUserIdAsync(int userId);
        Task<UserWorkoutSession?> GetSessionWithLogsAsync(int sessionId);
        Task<int> GetCompletedCountAsync(int userId);
        Task<IEnumerable<UserWorkoutSession>> GetRecentAsync(int userId, int count = 7);
    }
}