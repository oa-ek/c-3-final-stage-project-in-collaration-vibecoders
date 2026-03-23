using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId) =>
            await _dbSet
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.ScheduledAt)
                .ToListAsync();

        public async Task<IEnumerable<Notification>> GetUnreadAsync(int userId) =>
            await _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

        public async Task MarkAsReadAsync(int notificationId)
        {
            var n = await _dbSet.FindAsync(notificationId);
            if (n != null)
            {
                n.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}