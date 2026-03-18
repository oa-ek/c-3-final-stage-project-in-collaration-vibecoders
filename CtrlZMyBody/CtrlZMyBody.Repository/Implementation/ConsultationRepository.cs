using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class ConsultationRepository : Repository<Consultation>, IConsultationRepository
    {
        public ConsultationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Consultation>> GetByUserIdAsync(int userId) =>
            await _dbSet
                .Include(c => c.Specialist)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.RequestedAt)
                .ToListAsync();

        public async Task<IEnumerable<Consultation>> GetPendingAsync() =>
            await _dbSet
                .Include(c => c.User)
                .Where(c => c.Status == "pending")
                .OrderBy(c => c.RequestedAt)
                .ToListAsync();

        public async Task<IEnumerable<Consultation>> GetBySpecialistAsync(int specialistId) =>
            await _dbSet
                .Include(c => c.User)
                .Where(c => c.SpecialistId == specialistId)
                .OrderByDescending(c => c.RequestedAt)
                .ToListAsync();
    }
}