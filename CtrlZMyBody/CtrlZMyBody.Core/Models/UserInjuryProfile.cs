using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class UserInjuryProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int InjuryTypeId { get; set; }

        [Required]
        public int DifficultyLevelId { get; set; }

        [Required]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(InjuryTypeId))]
        public virtual InjuryType? InjuryType { get; set; }

        [ForeignKey(nameof(DifficultyLevelId))]
        public virtual DifficultyLevel? DifficultyLevel { get; set; }
    }
}