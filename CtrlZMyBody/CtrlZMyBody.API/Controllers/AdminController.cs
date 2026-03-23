using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        // ─── USERS ─────────────────────────────────────────────────────────────

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.Users
                .Select(u => new {
                    u.UserId,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Phone,
                    u.Role,
                    u.IsActive,
                    u.CreatedAt
                })
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
            return Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest(new { message = "Email вже існує" });

            var user = new User
            {
                Email = req.Email,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Phone = req.Phone,
                Role = req.Role ?? "user",
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { user.UserId, user.Email, user.FullName, user.Role });
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AdminUpdateUserRequest req)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Phone = req.Phone;
            user.Role = req.Role;
            user.IsActive = req.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(req.NewPassword))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);

            await _db.SaveChangesAsync();
            return Ok(new { user.UserId, user.Email, user.FullName, user.Role, user.IsActive });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── EXERCISE CATEGORIES ───────────────────────────────────────────────

        [HttpGet("exercise-categories")]
        public async Task<IActionResult> GetExerciseCategories()
        {
            var list = await _db.ExerciseCategories.ToListAsync();
            return Ok(list);
        }

        [HttpPost("exercise-categories")]
        public async Task<IActionResult> CreateExerciseCategory([FromBody] ExerciseCategory req)
        {
            req.CategoryId = 0;
            _db.ExerciseCategories.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("exercise-categories/{id}")]
        public async Task<IActionResult> UpdateExerciseCategory(int id, [FromBody] ExerciseCategory req)
        {
            var item = await _db.ExerciseCategories.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = req.Name;
            item.Description = req.Description;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("exercise-categories/{id}")]
        public async Task<IActionResult> DeleteExerciseCategory(int id)
        {
            var item = await _db.ExerciseCategories.FindAsync(id);
            if (item == null) return NotFound();
            _db.ExerciseCategories.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── EXERCISES ─────────────────────────────────────────────────────────

        [HttpGet("exercises")]
        public async Task<IActionResult> GetExercises()
        {
            var list = await _db.Exercises
                .Include(e => e.Category)
                .Select(e => new {
                    e.ExerciseId,
                    e.Title,
                    e.Description,
                    e.CategoryId,
                    CategoryName = e.Category != null ? e.Category.Name : null,
                    e.VideoUrl,
                    e.PhotoUrl,
                    e.DefaultDurationSec,
                    e.DefaultSets,
                    e.DefaultReps,
                    e.Difficulty,
                    e.IsActive
                })
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost("exercises")]
        public async Task<IActionResult> CreateExercise([FromBody] Exercise req)
        {
            req.ExerciseId = 0;
            _db.Exercises.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("exercises/{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] Exercise req)
        {
            var item = await _db.Exercises.FindAsync(id);
            if (item == null) return NotFound();
            item.Title = req.Title;
            item.Description = req.Description;
            item.CategoryId = req.CategoryId;
            item.VideoUrl = req.VideoUrl;
            item.PhotoUrl = req.PhotoUrl;
            item.DefaultDurationSec = req.DefaultDurationSec;
            item.DefaultSets = req.DefaultSets;
            item.DefaultReps = req.DefaultReps;
            item.Difficulty = req.Difficulty;
            item.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("exercises/{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var item = await _db.Exercises.FindAsync(id);
            if (item == null) return NotFound();
            _db.Exercises.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── INJURY TYPES ──────────────────────────────────────────────────────

        [HttpGet("injury-types")]
        public async Task<IActionResult> GetInjuryTypes()
        {
            var list = await _db.InjuryTypes.ToListAsync();
            return Ok(list);
        }

        [HttpPost("injury-types")]
        public async Task<IActionResult> CreateInjuryType([FromBody] InjuryType req)
        {
            req.InjuryTypeId = 0;
            _db.InjuryTypes.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("injury-types/{id}")]
        public async Task<IActionResult> UpdateInjuryType(int id, [FromBody] InjuryType req)
        {
            var item = await _db.InjuryTypes.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = req.Name;
            item.Slug = req.Slug;
            item.Description = req.Description;
            item.BodyPart = req.BodyPart;
            item.IconUrl = req.IconUrl;
            item.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("injury-types/{id}")]
        public async Task<IActionResult> DeleteInjuryType(int id)
        {
            var item = await _db.InjuryTypes.FindAsync(id);
            if (item == null) return NotFound();
            _db.InjuryTypes.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── DIFFICULTY LEVELS ─────────────────────────────────────────────────

        [HttpGet("difficulty-levels")]
        public async Task<IActionResult> GetDifficultyLevels()
        {
            var list = await _db.DifficultyLevels.OrderBy(d => d.OrderIndex).ToListAsync();
            return Ok(list);
        }

        [HttpPost("difficulty-levels")]
        public async Task<IActionResult> CreateDifficultyLevel([FromBody] DifficultyLevel req)
        {
            req.DifficultyLevelId = 0;
            _db.DifficultyLevels.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("difficulty-levels/{id}")]
        public async Task<IActionResult> UpdateDifficultyLevel(int id, [FromBody] DifficultyLevel req)
        {
            var item = await _db.DifficultyLevels.FindAsync(id);
            if (item == null) return NotFound();
            item.Name = req.Name;
            item.Slug = req.Slug;
            item.OrderIndex = req.OrderIndex;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("difficulty-levels/{id}")]
        public async Task<IActionResult> DeleteDifficultyLevel(int id)
        {
            var item = await _db.DifficultyLevels.FindAsync(id);
            if (item == null) return NotFound();
            _db.DifficultyLevels.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── WORKOUT PLANS ─────────────────────────────────────────────────────

        [HttpGet("workout-plans")]
        public async Task<IActionResult> GetWorkoutPlans()
        {
            var list = await _db.WorkoutPlans
                .Include(p => p.InjuryType)
                .Include(p => p.DifficultyLevel)
                .Select(p => new {
                    p.PlanId,
                    p.Title,
                    p.Description,
                    p.TotalDays,
                    p.IsActive,
                    p.InjuryTypeId,
                    InjuryTypeName = p.InjuryType != null ? p.InjuryType.Name : null,
                    p.DifficultyLevelId,
                    DifficultyLevelName = p.DifficultyLevel != null ? p.DifficultyLevel.Name : null
                })
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost("workout-plans")]
        public async Task<IActionResult> CreateWorkoutPlan([FromBody] WorkoutPlan req)
        {
            req.PlanId = 0;
            _db.WorkoutPlans.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("workout-plans/{id}")]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, [FromBody] WorkoutPlan req)
        {
            var item = await _db.WorkoutPlans.FindAsync(id);
            if (item == null) return NotFound();
            item.Title = req.Title;
            item.Description = req.Description;
            item.TotalDays = req.TotalDays;
            item.IsActive = req.IsActive;
            item.InjuryTypeId = req.InjuryTypeId;
            item.DifficultyLevelId = req.DifficultyLevelId;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("workout-plans/{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            var item = await _db.WorkoutPlans.FindAsync(id);
            if (item == null) return NotFound();
            _db.WorkoutPlans.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }

        // ─── CHALLENGES ────────────────────────────────────────────────────────

        [HttpGet("challenges")]
        public async Task<IActionResult> GetChallenges()
        {
            var list = await _db.Challenges.ToListAsync();
            return Ok(list);
        }

        [HttpPost("challenges")]
        public async Task<IActionResult> CreateChallenge([FromBody] Challenge req)
        {
            req.ChallengeId = 0;
            _db.Challenges.Add(req);
            await _db.SaveChangesAsync();
            return Ok(req);
        }

        [HttpPut("challenges/{id}")]
        public async Task<IActionResult> UpdateChallenge(int id, [FromBody] Challenge req)
        {
            var item = await _db.Challenges.FindAsync(id);
            if (item == null) return NotFound();
            item.Title = req.Title;
            item.Description = req.Description;
            item.Type = req.Type;
            item.StartDate = req.StartDate;
            item.EndDate = req.EndDate;
            item.GoalValue = req.GoalValue;
            item.GoalMetric = req.GoalMetric;
            item.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("challenges/{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            var item = await _db.Challenges.FindAsync(id);
            if (item == null) return NotFound();
            _db.Challenges.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Видалено" });
        }
    }

    // ─── REQUEST MODELS ────────────────────────────────────────────────────────

    public record AdminCreateUserRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string? Phone,
        string? Role
    );

    public record AdminUpdateUserRequest(
        string FirstName,
        string LastName,
        string? Phone,
        string Role,
        bool IsActive,
        string? NewPassword
    );
}