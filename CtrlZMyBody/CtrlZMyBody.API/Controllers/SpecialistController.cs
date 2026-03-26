using System.ComponentModel.DataAnnotations;
using CtrlZMyBody.API.Models;
using CtrlZMyBody.Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CtrlZMyBody.API.Controllers
{
    [Route("specialist")]
    public class SpecialistController : Controller
    {
        private readonly AppDbContext _db;

        public SpecialistController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var guard = EnsureSpecialistAccess();
            if (guard != null)
            {
                return guard;
            }

            ViewData["Title"] = "Кабінет спеціаліста";
            ViewData["BodyClass"] = "specialist-dashboard-page";

            var specialistId = HttpContext.Session.GetInt32("UserId")!.Value;
            var pending = await _db.Consultations
                .AsNoTracking()
                .Include(c => c.User)
                .Where(c => c.Status == "pending")
                .OrderBy(c => c.RequestedAt)
                .ToListAsync();

            var mine = await _db.Consultations
                .AsNoTracking()
                .Include(c => c.User)
                .Where(c => c.SpecialistId == specialistId)
                .OrderByDescending(c => c.RespondedAt ?? c.RequestedAt)
                .Take(10)
                .ToListAsync();

            return View(new SpecialistDashboardViewModel
            {
                PendingConsultations = pending,
                MyRecentConsultations = mine
            });
        }

        [HttpPost("respond")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(RespondConsultationFormModel form)
        {
            var guard = EnsureSpecialistAccess();
            if (guard != null)
            {
                return guard;
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Відповідь не може бути порожньою.";
                return Redirect("/specialist");
            }

            var specialistId = HttpContext.Session.GetInt32("UserId")!.Value;
            var consultation = await _db.Consultations
                .FirstOrDefaultAsync(c => c.ConsultationId == form.ConsultationId);

            if (consultation == null)
            {
                TempData["Error"] = "Запит не знайдено.";
                return Redirect("/specialist");
            }

            consultation.SpecialistId = specialistId;
            consultation.ResponseText = form.Response;
            consultation.RespondedAt = DateTime.UtcNow;
            consultation.Status = "completed";

            await _db.SaveChangesAsync();
            TempData["Success"] = "Консультацію успішно надано.";
            return Redirect("/specialist");
        }

        private IActionResult? EnsureSpecialistAccess()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");
            if (userId == null)
            {
                return Redirect("/account/login?returnUrl=/specialist");
            }

            if (!string.Equals(role, "specialist", StringComparison.OrdinalIgnoreCase))
            {
                return role switch
                {
                    "user" => Redirect("/user"),
                    "admin" => Redirect("/admin/users"),
                    _ => Redirect("/")
                };
            }

            return null;
        }
    }

    public class RespondConsultationFormModel
    {
        public int ConsultationId { get; set; }

        [Required(ErrorMessage = "Вкажіть відповідь.")]
        public string Response { get; set; } = string.Empty;
    }
}
