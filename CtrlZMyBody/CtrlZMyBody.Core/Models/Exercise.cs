using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class ExerciseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; } = [];
    }

    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseId { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? VideoUrl { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        public int DefaultDurationSec { get; set; } = 30;
        public int DefaultSets { get; set; } = 3;
        public int DefaultReps { get; set; } = 10;

        [MaxLength(20)]
        public string Difficulty { get; set; } = "beginner";

        [Required]
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(CategoryId))]
        public virtual ExerciseCategory? Category { get; set; }

        public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = [];
        public virtual ICollection<UserExerciseLog> ExerciseLogs { get; set; } = [];
    }
}