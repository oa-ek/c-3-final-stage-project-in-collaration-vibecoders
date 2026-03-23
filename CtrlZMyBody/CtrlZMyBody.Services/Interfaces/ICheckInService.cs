using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface ICheckInService
    {
        Task<DailyCheckIn> CreateCheckInAsync(int userId, int painLevel,
            string? mood, string? comment, string? photoBeforeUrl, string? photoAfterUrl);
        Task<DailyCheckIn?> GetTodayCheckInAsync(int userId);
        Task<IEnumerable<DailyCheckIn>> GetHistoryAsync(int userId);
        Task<IEnumerable<DailyCheckIn>> GetRangeAsync(int userId, DateOnly from, DateOnly to);
    }
}