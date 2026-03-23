using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class InjuryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InjuryTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? BodyPart { get; set; }

        [MaxLength(500)]
        public string? IconUrl { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation
        public virtual ICollection<UserInjuryProfile> UserProfiles { get; set; } = [];
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = [];
    }
}