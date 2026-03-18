using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public ChallengeController(IChallengeService challengeService) =>
            _challengeService = challengeService;

        // GET api/challenge/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var challenges = await _challengeService.GetActiveChallengesAsync();
            return Ok(challenges);
        }

        // POST api/challenge/{challengeId}/join
        [HttpPost("{challengeId}/join")]
        public async Task<IActionResult> Join(int challengeId)
        {
            try
            {
                var uc = await _challengeService.JoinChallengeAsync(CurrentUserId, challengeId);
                return Ok(uc);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/challenge/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyChallenges()
        {
            var challenges = await _challengeService.GetUserChallengesAsync(CurrentUserId);
            return Ok(challenges);
        }

        // GET api/challenge/{challengeId}/leaderboard
        [HttpGet("{challengeId}/leaderboard")]
        public async Task<IActionResult> GetLeaderboard(int challengeId)
        {
            var board = await _challengeService.GetLeaderboardAsync(challengeId);
            return Ok(board);
        }
    }
}