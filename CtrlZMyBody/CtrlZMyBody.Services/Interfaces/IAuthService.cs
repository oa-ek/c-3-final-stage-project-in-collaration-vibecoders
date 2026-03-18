using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(User user, string token)> RegisterAsync(string email, string password,
            string firstName, string lastName, string? phone = null);
        Task<(User user, string token)> LoginAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
}