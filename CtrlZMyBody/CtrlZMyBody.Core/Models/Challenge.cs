using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class Challenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChallengeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = "weekly";

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public int GoalValue { get; set; }
        [Required]
        [MaxLength(50)]
        public string GoalMetric { get; set; } = "exercises_completed";

        [MaxLength(500)]
        public string? BadgeIconUrl { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<UserChallenge> Participants { get; set; } = [];
    }

    public class UserChallenge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserChallengeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        public int CurrentValue { get; set; } = 0;

        [Required]
        public bool IsCompleted { get; set; } = false;

        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(ChallengeId))]
        public virtual Challenge? Challenge { get; set; }
    }

    public class PointTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        [MaxLength(50)]
        public string SourceType { get; set; } = string.Empty;

        public int? SourceId { get; set; }

        [Required]
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }

    public class Consultation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsultationId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? SpecialistId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        public string? ProblemDescription { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }
        public string? ResponseText { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(SpecialistId))]
        public virtual User? Specialist { get; set; }
    }

    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Body { get; set; }
        [Required]
        [MaxLength(20)]
        public string Channel { get; set; } = "push";

        [Required]
        public bool IsSent { get; set; } = false;

        [Required]
        public bool IsRead { get; set; } = false;

        public DateTime? ScheduledAt { get; set; }
        public DateTime? SentAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}