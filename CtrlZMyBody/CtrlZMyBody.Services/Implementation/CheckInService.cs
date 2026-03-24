using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class CheckInService : ICheckInService
    {
        private readonly IDailyCheckInRepository _repo;

        public CheckInService(IDailyCheckInRepository repo) => _repo = repo;

        public async Task<DailyCheckIn> CreateCheckInAsync(int userId, int painLevel,
            string? mood, string? comment, string? photoBeforeUrl, string? photoAfterUrl)
        {
            var existing = await _repo.GetTodayCheckInAsync(userId);
            if (existing != null)
            {
                existing.PainLevel = painLevel;
                existing.Mood = mood;
                existing.Comment = comment;
                if (photoBeforeUrl != null) existing.PhotoBeforeUrl = photoBeforeUrl;
                if (photoAfterUrl != null) existing.PhotoAfterUrl = photoAfterUrl;
                await _repo.UpdateAsync(existing);
                return existing;
            }

            var checkIn = new DailyCheckIn
            {
                UserId = userId,
                CheckInDate = DateOnly.FromDateTime(DateTime.UtcNow),
                PainLevel = painLevel,
                Mood = mood,
                Comment = comment,
                PhotoBeforeUrl = photoBeforeUrl,
                PhotoAfterUrl = photoAfterUrl,
                CreatedAt = DateTime.UtcNow
            };

            return await _repo.AddAsync(checkIn);
        }

        public async Task<DailyCheckIn?> GetTodayCheckInAsync(int userId) =>
            await _repo.GetTodayCheckInAsync(userId);

        public async Task<IEnumerable<DailyCheckIn>> GetHistoryAsync(int userId) =>
            await _repo.GetByUserIdAsync(userId);

        public async Task<IEnumerable<DailyCheckIn>> GetRangeAsync(int userId, DateOnly from, DateOnly to) =>
            await _repo.GetRangeAsync(userId, from, to);
    }
}