using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}