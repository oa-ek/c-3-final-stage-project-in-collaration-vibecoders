using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckInController : ControllerBase
    {
        private readonly ICheckInService _checkInService;
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public CheckInController(ICheckInService checkInService) =>
            _checkInService = checkInService;

        // POST api/checkin
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCheckInRequest req)
        {
            var checkIn = await _checkInService.CreateCheckInAsync(
                CurrentUserId, req.PainLevel, req.Mood,
                req.Comment, req.PhotoBeforeUrl, req.PhotoAfterUrl);
            return Ok(checkIn);
        }

        // GET api/checkin/today
        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var checkIn = await _checkInService.GetTodayCheckInAsync(CurrentUserId);
            return Ok(checkIn);
        }

        // GET api/checkin/history
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _checkInService.GetHistoryAsync(CurrentUserId);
            return Ok(history);
        }

        // GET api/checkin/range?from=2025-01-01&to=2025-01-31
        [HttpGet("range")]
        public async Task<IActionResult> GetRange([FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            var data = await _checkInService.GetRangeAsync(CurrentUserId, from, to);
            return Ok(data);
        }
    }

    public record CreateCheckInRequest(int PainLevel, string? Mood,
        string? Comment, string? PhotoBeforeUrl, string? PhotoAfterUrl);
}