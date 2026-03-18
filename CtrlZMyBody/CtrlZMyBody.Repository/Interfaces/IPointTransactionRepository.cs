using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IPointTransactionRepository : IRepository<PointTransaction>
    {
        Task<IEnumerable<PointTransaction>> GetByUserIdAsync(int userId);
        Task<int> GetTotalPointsAsync(int userId);
    }
}