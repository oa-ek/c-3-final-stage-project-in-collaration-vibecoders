using Microsoft.AspNetCore.Mvc;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                var (user, token) = await _authService.RegisterAsync(
                    req.Email, req.Password, req.FirstName, req.LastName, req.Phone);

                return Ok(new
                {
                    userId = user.UserId,
                    email = user.Email,
                    fullName = user.FullName,
                    role = user.Role,
                    token
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            try
            {
                var (user, token) = await _authService.LoginAsync(req.Email, req.Password);
                return Ok(new
                {
                    userId = user.UserId,
                    email = user.Email,
                    fullName = user.FullName,
                    role = user.Role,
                    token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

    public record RegisterRequest(string Email, string Password,
        string FirstName, string LastName, string? Phone);
    public record LoginRequest(string Email, string Password);
}