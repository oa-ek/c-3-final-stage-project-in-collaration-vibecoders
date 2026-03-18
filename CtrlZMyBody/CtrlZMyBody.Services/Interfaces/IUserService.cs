using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetSpecialistsAsync();
    }
}