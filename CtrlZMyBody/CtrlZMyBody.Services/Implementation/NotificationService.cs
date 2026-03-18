using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo) => _repo = repo;

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId) =>
            await _repo.GetByUserIdAsync(userId);

        public async Task<IEnumerable<Notification>> GetUnreadAsync(int userId) =>
            await _repo.GetUnreadAsync(userId);

        public async Task MarkAsReadAsync(int notificationId) =>
            await _repo.MarkAsReadAsync(notificationId);

        public async Task CreateWorkoutReminderAsync(int userId, DateTime scheduledAt) =>
            await _repo.AddAsync(new Notification
            {
                UserId = userId,
                Type = "workout_reminder",
                Title = "Час для тренування!",
                Body = "Не забудь виконати свій план вправ на сьогодні.",
                Channel = "push",
                ScheduledAt = scheduledAt
            });
    }
}