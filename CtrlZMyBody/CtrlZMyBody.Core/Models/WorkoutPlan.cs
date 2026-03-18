using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class WorkoutPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanId { get; set; }

        [Required]
        public int InjuryTypeId { get; set; }

        [Required]
        public int DifficultyLevelId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int TotalDays { get; set; } = 28;

        [Required]
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(InjuryTypeId))]
        public virtual InjuryType? InjuryType { get; set; }

        [ForeignKey(nameof(DifficultyLevelId))]
        public virtual DifficultyLevel? DifficultyLevel { get; set; }

        public virtual ICollection<WorkoutPlanDay> Days { get; set; } = [];
    }

    public class WorkoutPlanDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanDayId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [Required]
        public int DayNumber { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        [ForeignKey(nameof(PlanId))]
        public virtual WorkoutPlan? Plan { get; set; }

        public virtual ICollection<WorkoutPlanExercise> Exercises { get; set; } = [];
        public virtual ICollection<UserWorkoutSession> Sessions { get; set; } = [];
    }

    public class WorkoutPlanExercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanExerciseId { get; set; }

        [Required]
        public int PlanDayId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        public int Sets { get; set; } = 3;
        public int Reps { get; set; } = 10;
        public int DurationSec { get; set; } = 30;
        public int OrderIndex { get; set; } = 1;

        [ForeignKey(nameof(PlanDayId))]
        public virtual WorkoutPlanDay? PlanDay { get; set; }

        [ForeignKey(nameof(ExerciseId))]
        public virtual Exercise? Exercise { get; set; }
    }
}