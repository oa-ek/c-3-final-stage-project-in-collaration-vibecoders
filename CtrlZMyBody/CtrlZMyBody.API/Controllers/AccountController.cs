using System.ComponentModel.DataAnnotations;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CtrlZMyBody.API.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        public const string RequiredDomainRegex = @"^[^@\s]+@ctrlz\.com$";
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsLoggedIn)
            {
                return RedirectToRoleHome(HttpContext.Session.GetString("UserRole"));
            }

            ViewData["BodyClass"] = "auth-page";
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginFormModel());
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginFormModel form, string? returnUrl = null)
        {
            ViewData["BodyClass"] = "auth-page";
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View("Login", form);
            }

            try
            {
                var (user, token) = await _authService.LoginAsync(form.Email, form.Password);
                SetSession(user, token);

                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToRoleHome(user.Role);
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Login", form);
            }
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (IsLoggedIn)
            {
                return RedirectToRoleHome(HttpContext.Session.GetString("UserRole"));
            }

            ViewData["BodyClass"] = "auth-page";
            return View(new RegisterFormModel());
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(RegisterFormModel form)
        {
            ViewData["BodyClass"] = "auth-page";

            if (!ModelState.IsValid)
            {
                return View("Register", form);
            }

            if (form.Password != form.ConfirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View("Register", form);
            }

            try
            {
                var (user, token) = await _authService.RegisterAsync(
                    form.Email, form.Password, form.FirstName, form.LastName, form.Phone);

                SetSession(user, token);
                return Redirect("/user");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Error = ex.Message;
                return View("Register", form);
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        private bool IsLoggedIn => HttpContext.Session.GetInt32("UserId") != null;

        private IActionResult RedirectToRoleHome(string? role)
        {
            return role switch
            {
                "admin" => Redirect("/admin/users"),
                "specialist" => Redirect("/specialist"),
                _ => Redirect("/user")
            };
        }

        private void SetSession(User user, string token)
        {
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserToken", token);

            if (string.Equals(user.Role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetInt32("AdminId", user.UserId);
                HttpContext.Session.SetString("AdminName", user.FullName);
                HttpContext.Session.SetString("AdminRole", "admin");
            }
            else
            {
                HttpContext.Session.Remove("AdminId");
                HttpContext.Session.Remove("AdminName");
                HttpContext.Session.Remove("AdminRole");
            }
        }
    }

    public class LoginFormModel
    {
        [Required(ErrorMessage = "Вкажіть email.")]
        [RegularExpression(AccountController.RequiredDomainRegex, ErrorMessage = "Email має бути у домені @ctrlz.com.")]
        [EmailAddress(ErrorMessage = "Некоректний email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть пароль.")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Вкажіть ім'я.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть прізвище.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть email.")]
        [RegularExpression(AccountController.RequiredDomainRegex, ErrorMessage = "Email має бути у домені @ctrlz.com.")]
        [EmailAddress(ErrorMessage = "Некоректний email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть пароль.")]
        [MinLength(6, ErrorMessage = "Пароль має містити мінімум 6 символів.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підтвердіть пароль.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}
