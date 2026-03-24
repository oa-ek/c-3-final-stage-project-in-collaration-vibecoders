using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public WorkoutController(IWorkoutService workoutService) =>
            _workoutService = workoutService;
        [HttpGet("plan")]
        public async Task<IActionResult> GetPlan([FromQuery] int injuryTypeId,
            [FromQuery] int difficultyLevelId)
        {
            var plan = await _workoutService.GetPlanForUserAsync(injuryTypeId, difficultyLevelId);
            if (plan == null) return NotFound(new { message = "План не знайдено." });
            return Ok(plan);
        }
        [HttpGet("day/{planDayId}")]
        public async Task<IActionResult> GetDay(int planDayId)
        {
            var day = await _workoutService.GetDayAsync(planDayId);
            if (day == null) return NotFound(new { message = "День не знайдено." });
            return Ok(day);
        }
        [HttpPost("session/start")]
        public async Task<IActionResult> StartSession([FromBody] StartSessionRequest req)
        {
            var session = await _workoutService.StartSessionAsync(CurrentUserId, req.PlanDayId);
            return Ok(session);
        }
        [HttpPost("session/{sessionId}/complete-exercise")]
        public async Task<IActionResult> CompleteExercise(int sessionId,
            [FromBody] CompleteExerciseRequest req)
        {
            var session = await _workoutService.CompleteExerciseAsync(
                sessionId, req.ExerciseId, req.Sets, req.Reps, req.DurationSec);
            return Ok(session);
        }
        [HttpPost("session/{sessionId}/finish")]
        public async Task<IActionResult> FinishSession(int sessionId)
        {
            var session = await _workoutService.FinishSessionAsync(sessionId);
            return Ok(session);
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _workoutService.GetUserHistoryAsync(CurrentUserId);
            return Ok(history);
        }
        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetSession(int sessionId)
        {
            var session = await _workoutService.GetSessionAsync(sessionId);
            if (session == null) return NotFound();
            return Ok(session);
        }
    }

    public record StartSessionRequest(int PlanDayId);
    public record CompleteExerciseRequest(int ExerciseId, int Sets, int Reps, int DurationSec);
}