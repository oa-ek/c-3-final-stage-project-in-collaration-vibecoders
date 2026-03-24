using System;
using CtrlZMyBody.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CtrlZMyBody.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Challenge", b =>
                {
                    b.Property<int>("ChallengeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ChallengeId"));

                    b.Property<string>("BadgeIconUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("GoalMetric")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("GoalValue")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("ChallengeId");

                    b.ToTable("Challenges");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Consultation", b =>
                {
                    b.Property<int>("ConsultationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ConsultationId"));

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("ProblemDescription")
                        .HasColumnType("text");

                    b.Property<DateTime>("RequestedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RespondedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ResponseText")
                        .HasColumnType("text");

                    b.Property<int?>("SpecialistId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ConsultationId");

                    b.HasIndex("SpecialistId");

                    b.HasIndex("UserId");

                    b.ToTable("Consultations");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.DailyCheckIn", b =>
                {
                    b.Property<int>("CheckInId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CheckInId"));

                    b.Property<DateOnly>("CheckInDate")
                        .HasColumnType("date");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Mood")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("PainLevel")
                        .HasColumnType("integer");

                    b.Property<string>("PhotoAfterUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("PhotoBeforeUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("CheckInId");

                    b.HasIndex("CheckInDate");

                    b.HasIndex("UserId");

                    b.ToTable("DailyCheckIns");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.DifficultyLevel", b =>
                {
                    b.Property<int>("DifficultyLevelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DifficultyLevelId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("DifficultyLevelId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("DifficultyLevels");

                    b.HasData(
                        new
                        {
                            DifficultyLevelId = 1,
                            Name = "Початковий",
                            OrderIndex = 1,
                            Slug = "beginner"
                        },
                        new
                        {
                            DifficultyLevelId = 2,
                            Name = "Середній",
                            OrderIndex = 2,
                            Slug = "intermediate"
                        },
                        new
                        {
                            DifficultyLevelId = 3,
                            Name = "Складний",
                            OrderIndex = 3,
                            Slug = "advanced"
                        });
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ExerciseId"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("DefaultDurationSec")
                        .HasColumnType("integer");

                    b.Property<int>("DefaultReps")
                        .HasColumnType("integer");

                    b.Property<int>("DefaultSets")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("VideoUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("ExerciseId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.ExerciseCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("CategoryId");

                    b.ToTable("ExerciseCategories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Description = "Вправи на гнучкість",
                            Name = "Розтяжка"
                        },
                        new
                        {
                            CategoryId = 2,
                            Description = "Силові вправи",
                            Name = "Зміцнення"
                        },
                        new
                        {
                            CategoryId = 3,
                            Description = "Координація та рівновага",
                            Name = "Баланс"
                        },
                        new
                        {
                            CategoryId = 4,
                            Description = "Покращення рухливості",
                            Name = "Мобільність суглобів"
                        });
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.InjuryType", b =>
                {
                    b.Property<int>("InjuryTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("InjuryTypeId"));

                    b.Property<string>("BodyPart")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("IconUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("InjuryTypeId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("InjuryTypes");

                    b.HasData(
                        new
                        {
                            InjuryTypeId = 1,
                            BodyPart = "knee",
                            IsActive = true,
                            Name = "Розрив зв'язок коліна",
                            Slug = "knee-ligament"
                        },
                        new
                        {
                            InjuryTypeId = 2,
                            BodyPart = "back",
                            IsActive = true,
                            Name = "Грижа попереку",
                            Slug = "lower-back-hernia"
                        },
                        new
                        {
                            InjuryTypeId = 3,
                            BodyPart = "shoulder",
                            IsActive = true,
                            Name = "Пошкодження плеча",
                            Slug = "shoulder-injury"
                        },
                        new
                        {
                            InjuryTypeId = 4,
                            BodyPart = "ankle",
                            IsActive = true,
                            Name = "Розтягнення гомілкостопу",
                            Slug = "ankle-sprain"
                        });
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("NotificationId"));

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSent")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ScheduledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.PointTransaction", b =>
                {
                    b.Property<int>("PointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PointId"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EarnedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("SourceId")
                        .HasColumnType("integer");

                    b.Property<string>("SourceType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PointId");

                    b.HasIndex("UserId");

                    b.ToTable("PointTransactions");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("ResetToken")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Role");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserChallenge", b =>
                {
                    b.Property<int>("UserChallengeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserChallengeId"));

                    b.Property<int>("ChallengeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CurrentValue")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserChallengeId");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("UserId", "ChallengeId")
                        .IsUnique();

                    b.ToTable("UserChallenges");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserExerciseLog", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LogId"));

                    b.Property<int>("ActualDurationSec")
                        .HasColumnType("integer");

                    b.Property<int>("ActualReps")
                        .HasColumnType("integer");

                    b.Property<int>("ActualSets")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.HasKey("LogId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("SessionId");

                    b.ToTable("UserExerciseLogs");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserInjuryProfile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProfileId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DifficultyLevelId")
                        .HasColumnType("integer");

                    b.Property<int>("InjuryTypeId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ProfileId");

                    b.HasIndex("DifficultyLevelId");

                    b.HasIndex("InjuryTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInjuryProfiles");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserWorkoutSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionId"));

                    b.Property<int>("CompletedExercises")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<int>("PlanDayId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("SessionDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TotalExercises")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("SessionId");

                    b.HasIndex("PlanDayId");

                    b.HasIndex("SessionDate");

                    b.HasIndex("UserId");

                    b.ToTable("UserWorkoutSessions");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlanId"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("DifficultyLevelId")
                        .HasColumnType("integer");

                    b.Property<int>("InjuryTypeId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("TotalDays")
                        .HasColumnType("integer");

                    b.HasKey("PlanId");

                    b.HasIndex("DifficultyLevelId");

                    b.HasIndex("InjuryTypeId");

                    b.ToTable("WorkoutPlans");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlanDay", b =>
                {
                    b.Property<int>("PlanDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlanDayId"));

                    b.Property<int>("DayNumber")
                        .HasColumnType("integer");

                    b.Property<int>("PlanId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("PlanDayId");

                    b.HasIndex("PlanId");

                    b.ToTable("WorkoutPlanDays");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlanExercise", b =>
                {
                    b.Property<int>("PlanExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlanExerciseId"));

                    b.Property<int>("DurationSec")
                        .HasColumnType("integer");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer");

                    b.Property<int>("PlanDayId")
                        .HasColumnType("integer");

                    b.Property<int>("Reps")
                        .HasColumnType("integer");

                    b.Property<int>("Sets")
                        .HasColumnType("integer");

                    b.HasKey("PlanExerciseId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("PlanDayId");

                    b.ToTable("WorkoutPlanExercises");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Consultation", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.User", "Specialist")
                        .WithMany()
                        .HasForeignKey("SpecialistId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Specialist");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.DailyCheckIn", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("DailyCheckIns")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Exercise", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.ExerciseCategory", "Category")
                        .WithMany("Exercises")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Notification", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.PointTransaction", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("Points")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserChallenge", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.Challenge", "Challenge")
                        .WithMany("Participants")
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("Challenges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Challenge");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserExerciseLog", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.Exercise", "Exercise")
                        .WithMany("ExerciseLogs")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.UserWorkoutSession", "Session")
                        .WithMany("ExerciseLogs")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserInjuryProfile", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.DifficultyLevel", "DifficultyLevel")
                        .WithMany("UserProfiles")
                        .HasForeignKey("DifficultyLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.InjuryType", "InjuryType")
                        .WithMany("UserProfiles")
                        .HasForeignKey("InjuryTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("InjuryProfiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DifficultyLevel");

                    b.Navigation("InjuryType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserWorkoutSession", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.WorkoutPlanDay", "PlanDay")
                        .WithMany("Sessions")
                        .HasForeignKey("PlanDayId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.User", "User")
                        .WithMany("WorkoutSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlanDay");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlan", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.DifficultyLevel", "DifficultyLevel")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("DifficultyLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.InjuryType", "InjuryType")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("InjuryTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DifficultyLevel");

                    b.Navigation("InjuryType");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlanDay", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.WorkoutPlan", "Plan")
                        .WithMany("Days")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlanExercise", b =>
                {
                    b.HasOne("CtrlZMyBody.Core.Models.Exercise", "Exercise")
                        .WithMany("WorkoutPlanExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CtrlZMyBody.Core.Models.WorkoutPlanDay", "PlanDay")
                        .WithMany("Exercises")
                        .HasForeignKey("PlanDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("PlanDay");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Challenge", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.DifficultyLevel", b =>
                {
                    b.Navigation("UserProfiles");

                    b.Navigation("WorkoutPlans");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.Exercise", b =>
                {
                    b.Navigation("ExerciseLogs");

                    b.Navigation("WorkoutPlanExercises");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.ExerciseCategory", b =>
                {
                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.InjuryType", b =>
                {
                    b.Navigation("UserProfiles");

                    b.Navigation("WorkoutPlans");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.User", b =>
                {
                    b.Navigation("Challenges");

                    b.Navigation("DailyCheckIns");

                    b.Navigation("InjuryProfiles");

                    b.Navigation("Notifications");

                    b.Navigation("Points");

                    b.Navigation("WorkoutSessions");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.UserWorkoutSession", b =>
                {
                    b.Navigation("ExerciseLogs");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlan", b =>
                {
                    b.Navigation("Days");
                });

            modelBuilder.Entity("CtrlZMyBody.Core.Models.WorkoutPlanDay", b =>
                {
                    b.Navigation("Exercises");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
