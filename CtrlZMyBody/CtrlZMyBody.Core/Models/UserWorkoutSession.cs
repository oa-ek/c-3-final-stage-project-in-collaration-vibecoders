using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class UserWorkoutSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PlanDayId { get; set; }

        [Required]
        public DateOnly SessionDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Required]
        public bool IsCompleted { get; set; } = false;

        public int TotalExercises { get; set; } = 0;
        public int CompletedExercises { get; set; } = 0;

        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(PlanDayId))]
        public virtual WorkoutPlanDay? PlanDay { get; set; }

        public virtual ICollection<UserExerciseLog> ExerciseLogs { get; set; } = [];
    }

    public class UserExerciseLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public bool IsCompleted { get; set; } = false;

        public int ActualSets { get; set; } = 0;
        public int ActualReps { get; set; } = 0;
        public int ActualDurationSec { get; set; } = 0;

        public DateTime? CompletedAt { get; set; }

        [ForeignKey(nameof(SessionId))]
        public virtual UserWorkoutSession? Session { get; set; }

        [ForeignKey(nameof(ExerciseId))]
        public virtual Exercise? Exercise { get; set; }
    }
}