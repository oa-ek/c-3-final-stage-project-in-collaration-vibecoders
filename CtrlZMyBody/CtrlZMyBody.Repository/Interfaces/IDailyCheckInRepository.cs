using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IDailyCheckInRepository : IRepository<DailyCheckIn>
    {
        Task<IEnumerable<DailyCheckIn>> GetByUserIdAsync(int userId);
        Task<DailyCheckIn?> GetTodayCheckInAsync(int userId);
        Task<IEnumerable<DailyCheckIn>> GetRangeAsync(int userId, DateOnly from, DateOnly to);
    }
}