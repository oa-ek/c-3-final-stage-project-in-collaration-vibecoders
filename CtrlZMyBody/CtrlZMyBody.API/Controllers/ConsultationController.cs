using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public ConsultationController(IConsultationService consultationService) =>
            _consultationService = consultationService;

        // POST api/consultation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConsultationRequest req)
        {
            var consultation = await _consultationService
                .CreateAsync(CurrentUserId, req.Problem, req.PhotoUrl);
            return Ok(consultation);
        }

        // GET api/consultation/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var list = await _consultationService.GetByUserAsync(CurrentUserId);
            return Ok(list);
        }

        // GET api/consultation/pending  (тільки для specialist/admin)
        [HttpGet("pending")]
        [Authorize(Roles = "specialist,admin")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _consultationService.GetPendingAsync();
            return Ok(list);
        }

        // POST api/consultation/{id}/respond  (тільки для specialist/admin)
        [HttpPost("{id}/respond")]
        [Authorize(Roles = "specialist,admin")]
        public async Task<IActionResult> Respond(int id, [FromBody] RespondRequest req)
        {
            var consultation = await _consultationService
                .RespondAsync(id, CurrentUserId, req.Response);
            return Ok(consultation);
        }
    }

    public record CreateConsultationRequest(string Problem, string? PhotoUrl);
    public record RespondRequest(string Response);
}