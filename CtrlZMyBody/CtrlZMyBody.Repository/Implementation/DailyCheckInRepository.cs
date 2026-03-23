using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class DailyCheckInRepository : Repository<DailyCheckIn>, IDailyCheckInRepository
    {
        public DailyCheckInRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<DailyCheckIn>> GetByUserIdAsync(int userId) =>
            await _dbSet
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CheckInDate)
                .ToListAsync();

        public async Task<DailyCheckIn?> GetTodayCheckInAsync(int userId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _dbSet
                .FirstOrDefaultAsync(c => c.UserId == userId && c.CheckInDate == today);
        }

        public async Task<IEnumerable<DailyCheckIn>> GetRangeAsync(int userId, DateOnly from, DateOnly to) =>
            await _dbSet
                .Where(c => c.UserId == userId && c.CheckInDate >= from && c.CheckInDate <= to)
                .OrderBy(c => c.CheckInDate)
                .ToListAsync();
    }
}