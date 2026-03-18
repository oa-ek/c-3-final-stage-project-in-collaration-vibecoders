using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
        Task<bool> EmailExistsAsync(string email);
    }
}