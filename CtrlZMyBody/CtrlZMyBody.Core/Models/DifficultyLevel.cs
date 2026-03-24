using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class DifficultyLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DifficultyLevelId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public int OrderIndex { get; set; } = 1;
        public virtual ICollection<UserInjuryProfile> UserProfiles { get; set; } = [];
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
    }
}