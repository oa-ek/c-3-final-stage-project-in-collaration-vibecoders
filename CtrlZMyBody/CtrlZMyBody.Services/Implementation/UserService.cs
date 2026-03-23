using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<User?> GetByIdAsync(int userId) =>
            await _userRepo.GetByIdAsync(userId);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepo.GetByEmailAsync(email);

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);
        }

        public async Task<IEnumerable<User>> GetSpecialistsAsync() =>
            await _userRepo.GetByRoleAsync("specialist");
    }
}