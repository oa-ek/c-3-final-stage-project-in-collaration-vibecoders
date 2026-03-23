using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CtrlZMyBody.API.Controllers
{
    [Route("admin")]
    public class AdminPanelController : Controller
    {
        private readonly AppDbContext _db;
        public AdminPanelController(AppDbContext db) => _db = db;

        private bool IsLoggedIn => HttpContext.Session.GetInt32("AdminId") != null;

        private IActionResult? CheckLogin()
            => IsLoggedIn ? null : Redirect("/admin/login");

        // ══════════════════════════════════════════════════════════════
        //  ЛОГІН / ВИХІД
        // ══════════════════════════════════════════════════════════════

        [HttpGet("login")]
        public IActionResult Login()
            => IsLoggedIn ? Redirect("/admin/users") : View();

        [HttpPost("login")]
        public async Task<IActionResult> LoginPost(
            [FromForm] string email, [FromForm] string password)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Role == "admin");

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Невірний email або пароль";
                return View("Login");
            }

            HttpContext.Session.SetInt32("AdminId", user.UserId);
            HttpContext.Session.SetString("AdminName", user.FullName);
            return Redirect("/admin/users");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/admin/login");
        }

        // ══════════════════════════════════════════════════════════════
        //  КОРИСТУВАЧІ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("users")]
        public async Task<IActionResult> Users(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.Users.FindAsync(editId.Value) : null;
            var list = await _db.Users
                .OrderByDescending(u => u.CreatedAt).ToListAsync();
            return View(list);
        }

        [HttpPost("users/save")]
        public async Task<IActionResult> UserSave([FromForm] UserFormModel form)
        {
            if (form.UserId == 0)
            {
                if (await _db.Users.AnyAsync(u => u.Email == form.Email))
                {
                    TempData["Error"] = "Користувач з таким email вже існує";
                    return Redirect("/admin/users");
                }
                _db.Users.Add(new User
                {
                    Email        = form.Email,
                    FirstName    = form.FirstName,
                    LastName     = form.LastName,
                    Phone        = form.Phone,
                    Role         = form.Role,
                    IsActive     = form.IsActive,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(form.Password ?? "changeme"),
                    CreatedAt    = DateTime.UtcNow,
                    UpdatedAt    = DateTime.UtcNow
                });
                TempData["Success"] = "Користувача додано";
            }
            else
            {
                var u = await _db.Users.FindAsync(form.UserId);
                if (u == null) return Redirect("/admin/users");
                u.FirstName = form.FirstName;
                u.LastName  = form.LastName;
                u.Phone     = form.Phone;
                u.Role      = form.Role;
                u.IsActive  = form.IsActive;
                u.UpdatedAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(form.Password))
                    u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(form.Password);
                TempData["Success"] = "Збережено";
            }
            await _db.SaveChangesAsync();
            return Redirect("/admin/users");
        }

        [HttpPost("users/delete/{id}")]
        public async Task<IActionResult> UserDelete(int id)
        {
            var item = await _db.Users.FindAsync(id);
            if (item != null) { _db.Users.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/users");
        }

        // ══════════════════════════════════════════════════════════════
        //  КАТЕГОРІЇ ВПРАВ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("categories")]
        public async Task<IActionResult> Categories(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.ExerciseCategories.FindAsync(editId.Value) : null;
            return View(await _db.ExerciseCategories.ToListAsync());
        }

        [HttpPost("categories/save")]
        public async Task<IActionResult> CategorySave(
            [FromForm] int CategoryId,
            [FromForm] string Name,
            [FromForm] string? Description)
        {
            if (CategoryId == 0)
                _db.ExerciseCategories.Add(new ExerciseCategory
                    { Name = Name, Description = Description });
            else
            {
                var item = await _db.ExerciseCategories.FindAsync(CategoryId);
                if (item != null) { item.Name = Name; item.Description = Description; }
            }
            await _db.SaveChangesAsync();
            TempData["Success"] = CategoryId == 0 ? "Додано" : "Збережено";
            return Redirect("/admin/categories");
        }

        [HttpPost("categories/delete/{id}")]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var item = await _db.ExerciseCategories.FindAsync(id);
            if (item != null) { _db.ExerciseCategories.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/categories");
        }

        // ══════════════════════════════════════════════════════════════
        //  ВПРАВИ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("exercises")]
        public async Task<IActionResult> Exercises(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.Exercises.FindAsync(editId.Value) : null;
            ViewBag.Categories = await _db.ExerciseCategories
                .Select(c => new SelectListItem
                    { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToListAsync();
            var list = await _db.Exercises.Include(e => e.Category).ToListAsync();
            return View(list);
        }

        [HttpPost("exercises/save")]
        public async Task<IActionResult> ExerciseSave([FromForm] ExerciseFormModel form)
        {
            if (form.ExerciseId == 0)
            {
                _db.Exercises.Add(new Exercise
                {
                    Title              = form.Title,
                    Description        = form.Description,
                    CategoryId         = form.CategoryId == 0 ? null : form.CategoryId,
                    Difficulty         = form.Difficulty,
                    DefaultSets        = form.DefaultSets,
                    DefaultReps        = form.DefaultReps,
                    DefaultDurationSec = form.DefaultDurationSec,
                    VideoUrl           = form.VideoUrl,
                    PhotoUrl           = form.PhotoUrl,
                    IsActive           = form.IsActive
                });
                TempData["Success"] = "Вправу додано";
            }
            else
            {
                var item = await _db.Exercises.FindAsync(form.ExerciseId);
                if (item == null) return Redirect("/admin/exercises");
                item.Title              = form.Title;
                item.Description        = form.Description;
                item.CategoryId         = form.CategoryId == 0 ? null : form.CategoryId;
                item.Difficulty         = form.Difficulty;
                item.DefaultSets        = form.DefaultSets;
                item.DefaultReps        = form.DefaultReps;
                item.DefaultDurationSec = form.DefaultDurationSec;
                item.VideoUrl           = form.VideoUrl;
                item.PhotoUrl           = form.PhotoUrl;
                item.IsActive           = form.IsActive;
                TempData["Success"] = "Збережено";
            }
            await _db.SaveChangesAsync();
            return Redirect("/admin/exercises");
        }

        [HttpPost("exercises/delete/{id}")]
        public async Task<IActionResult> ExerciseDelete(int id)
        {
            var item = await _db.Exercises.FindAsync(id);
            if (item != null) { _db.Exercises.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/exercises");
        }

        // ══════════════════════════════════════════════════════════════
        //  ТИПИ ТРАВМ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("injuries")]
        public async Task<IActionResult> InjuryTypes(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.InjuryTypes.FindAsync(editId.Value) : null;
            return View(await _db.InjuryTypes.ToListAsync());
        }

        [HttpPost("injuries/save")]
        public async Task<IActionResult> InjuryTypeSave([FromForm] InjuryTypeFormModel form)
        {
            if (form.InjuryTypeId == 0)
            {
                _db.InjuryTypes.Add(new InjuryType
                {
                    Name        = form.Name,
                    Slug        = form.Slug,
                    BodyPart    = form.BodyPart,
                    Description = form.Description,
                    IconUrl     = form.IconUrl,
                    IsActive    = form.IsActive
                });
                TempData["Success"] = "Додано";
            }
            else
            {
                var item = await _db.InjuryTypes.FindAsync(form.InjuryTypeId);
                if (item != null)
                {
                    item.Name        = form.Name;
                    item.Slug        = form.Slug;
                    item.BodyPart    = form.BodyPart;
                    item.Description = form.Description;
                    item.IconUrl     = form.IconUrl;
                    item.IsActive    = form.IsActive;
                }
                TempData["Success"] = "Збережено";
            }
            await _db.SaveChangesAsync();
            return Redirect("/admin/injuries");
        }

        [HttpPost("injuries/delete/{id}")]
        public async Task<IActionResult> InjuryTypeDelete(int id)
        {
            var item = await _db.InjuryTypes.FindAsync(id);
            if (item != null) { _db.InjuryTypes.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/injuries");
        }

        // ══════════════════════════════════════════════════════════════
        //  РІВНІ СКЛАДНОСТІ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("difficulties")]
        public async Task<IActionResult> DifficultyLevels(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.DifficultyLevels.FindAsync(editId.Value) : null;
            return View(await _db.DifficultyLevels.OrderBy(d => d.OrderIndex).ToListAsync());
        }

        [HttpPost("difficulties/save")]
        public async Task<IActionResult> DifficultyLevelSave(
            [FromForm] int DifficultyLevelId,
            [FromForm] string Name,
            [FromForm] string Slug,
            [FromForm] int OrderIndex)
        {
            if (DifficultyLevelId == 0)
                _db.DifficultyLevels.Add(new DifficultyLevel
                    { Name = Name, Slug = Slug, OrderIndex = OrderIndex });
            else
            {
                var item = await _db.DifficultyLevels.FindAsync(DifficultyLevelId);
                if (item != null)
                    { item.Name = Name; item.Slug = Slug; item.OrderIndex = OrderIndex; }
            }
            await _db.SaveChangesAsync();
            TempData["Success"] = DifficultyLevelId == 0 ? "Додано" : "Збережено";
            return Redirect("/admin/difficulties");
        }

        [HttpPost("difficulties/delete/{id}")]
        public async Task<IActionResult> DifficultyLevelDelete(int id)
        {
            var item = await _db.DifficultyLevels.FindAsync(id);
            if (item != null) { _db.DifficultyLevels.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/difficulties");
        }

        // ══════════════════════════════════════════════════════════════
        //  ПЛАНИ ТРЕНУВАНЬ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("plans")]
        public async Task<IActionResult> WorkoutPlans(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.WorkoutPlans.FindAsync(editId.Value) : null;
            ViewBag.InjuryTypes = await _db.InjuryTypes
                .Select(i => new SelectListItem
                    { Value = i.InjuryTypeId.ToString(), Text = i.Name })
                .ToListAsync();
            ViewBag.DifficultyLevels = await _db.DifficultyLevels
                .OrderBy(d => d.OrderIndex)
                .Select(d => new SelectListItem
                    { Value = d.DifficultyLevelId.ToString(), Text = d.Name })
                .ToListAsync();
            var list = await _db.WorkoutPlans
                .Include(p => p.InjuryType)
                .Include(p => p.DifficultyLevel)
                .ToListAsync();
            return View(list);
        }

        [HttpPost("plans/save")]
        public async Task<IActionResult> WorkoutPlanSave([FromForm] WorkoutPlanFormModel form)
        {
            if (form.PlanId == 0)
            {
                _db.WorkoutPlans.Add(new WorkoutPlan
                {
                    Title             = form.Title,
                    Description       = form.Description,
                    InjuryTypeId      = form.InjuryTypeId,
                    DifficultyLevelId = form.DifficultyLevelId,
                    TotalDays         = form.TotalDays,
                    IsActive          = form.IsActive
                });
                TempData["Success"] = "План додано";
            }
            else
            {
                var item = await _db.WorkoutPlans.FindAsync(form.PlanId);
                if (item == null) return Redirect("/admin/plans");
                item.Title             = form.Title;
                item.Description       = form.Description;
                item.InjuryTypeId      = form.InjuryTypeId;
                item.DifficultyLevelId = form.DifficultyLevelId;
                item.TotalDays         = form.TotalDays;
                item.IsActive          = form.IsActive;
                TempData["Success"] = "Збережено";
            }
            await _db.SaveChangesAsync();
            return Redirect("/admin/plans");
        }

        [HttpPost("plans/delete/{id}")]
        public async Task<IActionResult> WorkoutPlanDelete(int id)
        {
            var item = await _db.WorkoutPlans.FindAsync(id);
            if (item != null) { _db.WorkoutPlans.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/plans");
        }

        // ══════════════════════════════════════════════════════════════
        //  ЧЕЛЕНДЖІ
        // ══════════════════════════════════════════════════════════════

        [HttpGet("challenges")]
        public async Task<IActionResult> Challenges(int? editId)
        {
            var check = CheckLogin(); if (check != null) return check;
            ViewBag.EditItem = editId.HasValue
                ? await _db.Challenges.FindAsync(editId.Value) : null;
            return View(await _db.Challenges.ToListAsync());
        }

        [HttpPost("challenges/save")]
        public async Task<IActionResult> ChallengeSave([FromForm] ChallengeFormModel form)
        {
            if (form.ChallengeId == 0)
            {
                _db.Challenges.Add(new Challenge
                {
                    Title       = form.Title,
                    Description = form.Description,
                    Type        = form.Type,
                    StartDate   = form.StartDate,
                    EndDate     = form.EndDate,
                    GoalValue   = form.GoalValue,
                    GoalMetric  = form.GoalMetric,
                    IsActive    = form.IsActive
                });
                TempData["Success"] = "Челендж додано";
            }
            else
            {
                var item = await _db.Challenges.FindAsync(form.ChallengeId);
                if (item == null) return Redirect("/admin/challenges");
                item.Title       = form.Title;
                item.Description = form.Description;
                item.Type        = form.Type;
                item.StartDate   = form.StartDate;
                item.EndDate     = form.EndDate;
                item.GoalValue   = form.GoalValue;
                item.GoalMetric  = form.GoalMetric;
                item.IsActive    = form.IsActive;
                TempData["Success"] = "Збережено";
            }
            await _db.SaveChangesAsync();
            return Redirect("/admin/challenges");
        }

        [HttpPost("challenges/delete/{id}")]
        public async Task<IActionResult> ChallengeDelete(int id)
        {
            var item = await _db.Challenges.FindAsync(id);
            if (item != null) { _db.Challenges.Remove(item); await _db.SaveChangesAsync(); }
            TempData["Success"] = "Видалено";
            return Redirect("/admin/challenges");
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  FORM MODELS
    // ══════════════════════════════════════════════════════════════════════

    public class UserFormModel
    {
        public int     UserId    { get; set; }
        public string  Email     { get; set; } = "";
        public string? Password  { get; set; }
        public string  FirstName { get; set; } = "";
        public string  LastName  { get; set; } = "";
        public string? Phone     { get; set; }
        public string  Role      { get; set; } = "user";
        public bool    IsActive  { get; set; } = true;
    }

    public class ExerciseFormModel
    {
        public int     ExerciseId        { get; set; }
        public string  Title             { get; set; } = "";
        public string? Description       { get; set; }
        public int?    CategoryId        { get; set; }
        public string  Difficulty        { get; set; } = "beginner";
        public int     DefaultSets       { get; set; } = 3;
        public int     DefaultReps       { get; set; } = 10;
        public int     DefaultDurationSec { get; set; } = 30;
        public string? VideoUrl          { get; set; }
        public string? PhotoUrl          { get; set; }
        public bool    IsActive          { get; set; } = true;
    }

    public class InjuryTypeFormModel
    {
        public int     InjuryTypeId { get; set; }
        public string  Name         { get; set; } = "";
        public string  Slug         { get; set; } = "";
        public string? BodyPart     { get; set; }
        public string? Description  { get; set; }
        public string? IconUrl      { get; set; }
        public bool    IsActive     { get; set; } = true;
    }

    public class WorkoutPlanFormModel
    {
        public int     PlanId            { get; set; }
        public string  Title             { get; set; } = "";
        public string? Description       { get; set; }
        public int     InjuryTypeId      { get; set; }
        public int     DifficultyLevelId { get; set; }
        public int     TotalDays         { get; set; } = 28;
        public bool    IsActive          { get; set; } = true;
    }

    public class ChallengeFormModel
    {
        public int      ChallengeId  { get; set; }
        public string   Title        { get; set; } = "";
        public string?  Description  { get; set; }
        public string   Type         { get; set; } = "weekly";
        public DateOnly StartDate    { get; set; }
        public DateOnly EndDate      { get; set; }
        public int      GoalValue    { get; set; }
        public string   GoalMetric   { get; set; } = "exercises_completed";
        public bool     IsActive     { get; set; } = true;
    }
}

