using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
        Task CreateWorkoutReminderAsync(int userId, DateTime scheduledAt);
    }
}