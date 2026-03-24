using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Core.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<InjuryType> InjuryTypes => Set<InjuryType>();
        public DbSet<DifficultyLevel> DifficultyLevels => Set<DifficultyLevel>();
        public DbSet<UserInjuryProfile> UserInjuryProfiles => Set<UserInjuryProfile>();
        public DbSet<ExerciseCategory> ExerciseCategories => Set<ExerciseCategory>();
        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
        public DbSet<WorkoutPlanDay> WorkoutPlanDays => Set<WorkoutPlanDay>();
        public DbSet<WorkoutPlanExercise> WorkoutPlanExercises => Set<WorkoutPlanExercise>();
        public DbSet<UserWorkoutSession> UserWorkoutSessions => Set<UserWorkoutSession>();
        public DbSet<UserExerciseLog> UserExerciseLogs => Set<UserExerciseLog>();
        public DbSet<DailyCheckIn> DailyCheckIns => Set<DailyCheckIn>();
        public DbSet<Challenge> Challenges => Set<Challenge>();
        public DbSet<UserChallenge> UserChallenges => Set<UserChallenge>();
        public DbSet<PointTransaction> PointTransactions => Set<PointTransaction>();
        public DbSet<Consultation> Consultations => Set<Consultation>();
        public DbSet<Notification> Notifications => Set<Notification>();

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Role);
            modelBuilder.Entity<InjuryType>().HasIndex(i => i.Slug).IsUnique();
            modelBuilder.Entity<DifficultyLevel>().HasIndex(d => d.Slug).IsUnique();
            modelBuilder.Entity<UserInjuryProfile>()
                .HasOne(p => p.User).WithMany(u => u.InjuryProfiles)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserInjuryProfile>()
                .HasOne(p => p.InjuryType).WithMany(i => i.UserProfiles)
                .HasForeignKey(p => p.InjuryTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserInjuryProfile>()
                .HasOne(p => p.DifficultyLevel).WithMany(d => d.UserProfiles)
                .HasForeignKey(p => p.DifficultyLevelId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(p => p.InjuryType).WithMany(i => i.WorkoutPlans)
                .HasForeignKey(p => p.InjuryTypeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(p => p.DifficultyLevel).WithMany(d => d.WorkoutPlans)
                .HasForeignKey(p => p.DifficultyLevelId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkoutPlanDay>()
                .HasOne(d => d.Plan).WithMany(p => p.Days)
                .HasForeignKey(d => d.PlanId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(pe => pe.PlanDay).WithMany(d => d.Exercises)
                .HasForeignKey(pe => pe.PlanDayId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WorkoutPlanExercise>()
                .HasOne(pe => pe.Exercise).WithMany(e => e.WorkoutPlanExercises)
                .HasForeignKey(pe => pe.ExerciseId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserWorkoutSession>()
                .HasOne(s => s.User).WithMany(u => u.WorkoutSessions)
                .HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserWorkoutSession>()
                .HasOne(s => s.PlanDay).WithMany(d => d.Sessions)
                .HasForeignKey(s => s.PlanDayId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserWorkoutSession>().HasIndex(s => s.UserId);
            modelBuilder.Entity<UserWorkoutSession>().HasIndex(s => s.SessionDate);
            modelBuilder.Entity<UserExerciseLog>()
                .HasOne(l => l.Session).WithMany(s => s.ExerciseLogs)
                .HasForeignKey(l => l.SessionId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserExerciseLog>()
                .HasOne(l => l.Exercise).WithMany(e => e.ExerciseLogs)
                .HasForeignKey(l => l.ExerciseId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DailyCheckIn>()
                .HasOne(c => c.User).WithMany(u => u.DailyCheckIns)
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DailyCheckIn>().HasIndex(c => c.UserId);
            modelBuilder.Entity<DailyCheckIn>().HasIndex(c => c.CheckInDate);
            modelBuilder.Entity<UserChallenge>()
                .HasOne(uc => uc.User).WithMany(u => u.Challenges)
                .HasForeignKey(uc => uc.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserChallenge>()
                .HasOne(uc => uc.Challenge).WithMany(c => c.Participants)
                .HasForeignKey(uc => uc.ChallengeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserChallenge>()
                .HasIndex(uc => new { uc.UserId, uc.ChallengeId }).IsUnique();
            modelBuilder.Entity<PointTransaction>()
                .HasOne(p => p.User).WithMany(u => u.Points)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.User).WithMany()
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Consultation>()
                .HasOne(c => c.Specialist).WithMany()
                .HasForeignKey(c => c.SpecialistId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User).WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DifficultyLevel>().HasData(
                new DifficultyLevel { DifficultyLevelId = 1, Name = "Початковий", Slug = "beginner", OrderIndex = 1 },
                new DifficultyLevel { DifficultyLevelId = 2, Name = "Середній", Slug = "intermediate", OrderIndex = 2 },
                new DifficultyLevel { DifficultyLevelId = 3, Name = "Складний", Slug = "advanced", OrderIndex = 3 }
            );

            modelBuilder.Entity<InjuryType>().HasData(
                new InjuryType { InjuryTypeId = 1, Name = "Розрив зв'язок коліна", Slug = "knee-ligament", BodyPart = "knee" },
                new InjuryType { InjuryTypeId = 2, Name = "Грижа попереку", Slug = "lower-back-hernia", BodyPart = "back" },
                new InjuryType { InjuryTypeId = 3, Name = "Пошкодження плеча", Slug = "shoulder-injury", BodyPart = "shoulder" },
                new InjuryType { InjuryTypeId = 4, Name = "Розтягнення гомілкостопу", Slug = "ankle-sprain", BodyPart = "ankle" }
            );

            modelBuilder.Entity<ExerciseCategory>().HasData(
                new ExerciseCategory { CategoryId = 1, Name = "Розтяжка", Description = "Вправи на гнучкість" },
                new ExerciseCategory { CategoryId = 2, Name = "Зміцнення", Description = "Силові вправи" },
                new ExerciseCategory { CategoryId = 3, Name = "Баланс", Description = "Координація та рівновага" },
                new ExerciseCategory { CategoryId = 4, Name = "Мобільність суглобів", Description = "Покращення рухливості" }
            );
        }
    }
}