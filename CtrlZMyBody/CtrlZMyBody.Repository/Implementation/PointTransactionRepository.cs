using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class PointTransactionRepository : Repository<PointTransaction>, IPointTransactionRepository
    {
        public PointTransactionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PointTransaction>> GetByUserIdAsync(int userId) =>
            await _dbSet
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.EarnedAt)
                .ToListAsync();

        public async Task<int> GetTotalPointsAsync(int userId) =>
            await _dbSet
                .Where(p => p.UserId == userId)
                .SumAsync(p => p.Amount);
    }
}