using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class DailyCheckIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckInId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Required]
        [Range(1, 10)]
        public int PainLevel { get; set; }
        [MaxLength(20)]
        public string? Mood { get; set; }

        public string? Comment { get; set; }

        [MaxLength(500)]
        public string? PhotoBeforeUrl { get; set; }

        [MaxLength(500)]
        public string? PhotoAfterUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}