using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private const string RequiredDomain = "@ctrlz.com";
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<(User user, string token)> RegisterAsync(string email, string password,
            string firstName, string lastName, string? phone = null)
        {
            var normalizedEmail = NormalizeEmail(email);
            ValidateEmailDomain(normalizedEmail);

            if (await _userRepo.EmailExistsAsync(normalizedEmail))
                throw new InvalidOperationException("Користувач з таким email вже існує.");

            var user = new User
            {
                Email = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Role = "user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            var token = GenerateJwtToken(user);
            return (user, token);
        }

        public async Task<(User user, string token)> LoginAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(NormalizeEmail(email))
                ?? throw new UnauthorizedAccessException("Невірний email або пароль.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Акаунт деактивовано.");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Невірний email або пароль.");

            var token = GenerateJwtToken(user);
            return (user, token);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("fullName", user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string NormalizeEmail(string email) =>
            email.Trim().ToLowerInvariant();

        private static void ValidateEmailDomain(string email)
        {
            if (!email.EndsWith(RequiredDomain, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Дозволено лише email у домені {RequiredDomain}.");
            }
        }
    }
}
