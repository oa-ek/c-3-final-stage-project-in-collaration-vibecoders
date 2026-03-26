using CtrlZMyBody.API.Models;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CtrlZMyBody.API.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public UserController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? day = null)
        {
            var guard = EnsureUserAccess();
            if (guard != null)
            {
                return guard;
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            ViewData["Title"] = "Моя реабілітація";
            ViewData["BodyClass"] = "user-dashboard-page";

            var injuries = await _db.InjuryTypes
                .AsNoTracking()
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();

            var difficulties = await _db.DifficultyLevels
                .AsNoTracking()
                .OrderBy(d => d.OrderIndex)
                .ToListAsync();

            var activeProfile = await _db.UserInjuryProfiles
                .Include(p => p.InjuryType)
                .Include(p => p.DifficultyLevel)
                .Where(p => p.UserId == userId && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            WorkoutPlan? currentPlan = null;
            WorkoutPlanDay? currentDay = null;
            if (activeProfile != null)
            {
                currentPlan = await _db.WorkoutPlans
                    .AsNoTracking()
                    .Include(p => p.InjuryType)
                    .Include(p => p.DifficultyLevel)
                    .Include(p => p.Days)
                        .ThenInclude(d => d.Exercises)
                            .ThenInclude(pe => pe.Exercise)
                    .Where(p =>
                        p.IsActive &&
                        p.InjuryTypeId == activeProfile.InjuryTypeId &&
                        p.DifficultyLevelId == activeProfile.DifficultyLevelId)
                    .OrderByDescending(p => p.PlanId)
                    .FirstOrDefaultAsync();

                if (currentPlan != null && currentPlan.Days.Count > 0)
                {
                    currentDay = day.HasValue
                        ? currentPlan.Days.FirstOrDefault(d => d.DayNumber == day.Value)
                        : currentPlan.Days.OrderBy(d => d.DayNumber).FirstOrDefault();
                }
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var todayCheckIn = await _db.DailyCheckIns
                .AsNoTracking()
                .Where(c => c.UserId == userId && c.CheckInDate == today)
                .FirstOrDefaultAsync();

            var recentSessions = await _db.UserWorkoutSessions
                .AsNoTracking()
                .Include(s => s.PlanDay)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartedAt ?? s.FinishedAt)
                .Take(8)
                .ToListAsync();

            var consultations = await _db.Consultations
                .AsNoTracking()
                .Include(c => c.Specialist)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.RequestedAt)
                .Take(8)
                .ToListAsync();

            var challenges = await _db.Challenges
                .AsNoTracking()
                .Where(c => c.IsActive && c.StartDate <= today && c.EndDate >= today)
                .OrderBy(c => c.EndDate)
                .Take(6)
                .ToListAsync();

            return View(new UserDashboardViewModel
            {
                InjuryTypes = injuries,
                DifficultyLevels = difficulties,
                ActiveProfile = activeProfile,
                CurrentPlan = currentPlan,
                CurrentDay = currentDay,
                TodayCheckIn = todayCheckIn,
                RecentSessions = recentSessions,
                Consultations = consultations,
                ActiveChallenges = challenges
            });
        }

        [HttpPost("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(int injuryTypeId, int difficultyLevelId)
        {
            var guard = EnsureUserAccess();
            if (guard != null)
            {
                return guard;
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var injuryExists = await _db.InjuryTypes.AnyAsync(i => i.InjuryTypeId == injuryTypeId && i.IsActive);
            var difficultyExists = await _db.DifficultyLevels.AnyAsync(d => d.DifficultyLevelId == difficultyLevelId);
            if (!injuryExists || !difficultyExists)
            {
                TempData["Error"] = "Оберіть коректні значення травми та рівня.";
                return Redirect("/user");
            }

            var activeProfiles = await _db.UserInjuryProfiles
                .Where(p => p.UserId == userId && p.IsActive)
                .ToListAsync();

            foreach (var profile in activeProfiles)
            {
                profile.IsActive = false;
            }

            _db.UserInjuryProfiles.Add(new UserInjuryProfile
            {
                UserId = userId,
                InjuryTypeId = injuryTypeId,
                DifficultyLevelId = difficultyLevelId,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = "Профіль реабілітації оновлено.";
            return Redirect("/user");
        }

        [HttpPost("checkin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCheckIn(CheckInFormModel form, IFormFile? photoBefore, IFormFile? photoAfter)
        {
            var guard = EnsureUserAccess();
            if (guard != null)
            {
                return guard;
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Перевірте дані чекіну.";
                return Redirect("/user#checkin");
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            try
            {
                var beforeUrl = await SaveImageAsync(photoBefore, userId);
                var afterUrl = await SaveImageAsync(photoAfter, userId);

                var existing = await _db.DailyCheckIns
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.CheckInDate == today);

                if (existing == null)
                {
                    _db.DailyCheckIns.Add(new DailyCheckIn
                    {
                        UserId = userId,
                        CheckInDate = today,
                        PainLevel = form.PainLevel,
                        Mood = form.Mood,
                        Comment = form.Comment,
                        PhotoBeforeUrl = beforeUrl,
                        PhotoAfterUrl = afterUrl,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.PainLevel = form.PainLevel;
                    existing.Mood = form.Mood;
                    existing.Comment = form.Comment;
                    existing.PhotoBeforeUrl = beforeUrl ?? existing.PhotoBeforeUrl;
                    existing.PhotoAfterUrl = afterUrl ?? existing.PhotoAfterUrl;
                }

                await _db.SaveChangesAsync();
                TempData["Success"] = "Щоденний чекін збережено.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return Redirect("/user#checkin");
        }

        [HttpPost("consultation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConsultation(CreateConsultationFormModel form)
        {
            var guard = EnsureUserAccess();
            if (guard != null)
            {
                return guard;
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Опишіть проблему для консультації.";
                return Redirect("/user#consultations");
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            _db.Consultations.Add(new Consultation
            {
                UserId = userId,
                Status = "pending",
                ProblemDescription = form.Problem,
                PhotoUrl = form.PhotoUrl,
                RequestedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = "Запит на консультацію відправлено.";
            return Redirect("/user#consultations");
        }

        [HttpPost("exercise/complete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteExercise(CompleteExerciseFormModel form)
        {
            var guard = EnsureUserAccess();
            if (guard != null)
            {
                return guard;
            }

            var userId = HttpContext.Session.GetInt32("UserId")!.Value;
            var day = await _db.WorkoutPlanDays
                .Include(d => d.Exercises)
                .FirstOrDefaultAsync(d => d.PlanDayId == form.PlanDayId);

            if (day == null)
            {
                TempData["Error"] = "День плану не знайдено.";
                return Redirect("/user");
            }

            var sessionDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var session = await _db.UserWorkoutSessions
                .Include(s => s.ExerciseLogs)
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId &&
                    s.PlanDayId == form.PlanDayId &&
                    s.SessionDate == sessionDate &&
                    !s.IsCompleted);

            if (session == null)
            {
                session = new UserWorkoutSession
                {
                    UserId = userId,
                    PlanDayId = form.PlanDayId,
                    SessionDate = sessionDate,
                    StartedAt = DateTime.UtcNow,
                    TotalExercises = day.Exercises.Count,
                    IsCompleted = false
                };
                _db.UserWorkoutSessions.Add(session);
                await _db.SaveChangesAsync();
            }

            var existingLog = session.ExerciseLogs.FirstOrDefault(l => l.ExerciseId == form.ExerciseId);
            if (existingLog == null)
            {
                existingLog = new UserExerciseLog
                {
                    SessionId = session.SessionId,
                    ExerciseId = form.ExerciseId
                };
                _db.UserExerciseLogs.Add(existingLog);
                session.ExerciseLogs.Add(existingLog);
            }

            existingLog.IsCompleted = true;
            existingLog.ActualSets = form.Sets;
            existingLog.ActualReps = form.Reps;
            existingLog.ActualDurationSec = form.DurationSec;
            existingLog.CompletedAt = DateTime.UtcNow;

            session.CompletedExercises = session.ExerciseLogs.Count(e => e.IsCompleted);
            if (session.CompletedExercises >= session.TotalExercises && session.TotalExercises > 0)
            {
                session.IsCompleted = true;
                session.FinishedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Вправу позначено як виконану.";

            return Redirect($"/user?day={form.DayNumber}#rehab");
        }

        private IActionResult? EnsureUserAccess()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");
            if (userId == null)
            {
                return Redirect("/account/login?returnUrl=/user");
            }

            if (!string.Equals(role, "user", StringComparison.OrdinalIgnoreCase))
            {
                return role switch
                {
                    "specialist" => Redirect("/specialist"),
                    "admin" => Redirect("/admin/users"),
                    _ => Redirect("/")
                };
            }

            return null;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file, int userId)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !AllowedImageExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Дозволені формати фото: JPG, PNG, WEBP.");
            }

            var folderName = DateTime.UtcNow.ToString("yyyyMM");
            var relativeFolder = Path.Combine("uploads", "checkins", folderName);
            var targetFolder = Path.Combine(_environment.WebRootPath, relativeFolder);
            Directory.CreateDirectory(targetFolder);

            var fileName = $"{userId}_{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var filePath = Path.Combine(targetFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/checkins/{folderName}/{fileName}";
        }
    }

    public class CheckInFormModel
    {
        [System.ComponentModel.DataAnnotations.Range(1, 10)]
        public int PainLevel { get; set; }
        public string? Mood { get; set; }
        public string? Comment { get; set; }
    }

    public class CreateConsultationFormModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Problem { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }

    public class CompleteExerciseFormModel
    {
        public int PlanDayId { get; set; }
        public int DayNumber { get; set; }
        public int ExerciseId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int DurationSec { get; set; }
    }
}
