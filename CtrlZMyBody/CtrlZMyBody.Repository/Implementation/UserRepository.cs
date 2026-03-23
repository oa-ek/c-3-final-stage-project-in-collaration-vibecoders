using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> GetByRoleAsync(string role) =>
            await _dbSet.Where(u => u.Role == role && u.IsActive).ToListAsync();

        public async Task<bool> EmailExistsAsync(string email) =>
            await _dbSet.AnyAsync(u => u.Email == email);
    }
}