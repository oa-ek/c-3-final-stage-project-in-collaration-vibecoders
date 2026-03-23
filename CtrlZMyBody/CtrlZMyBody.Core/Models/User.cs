using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtrlZMyBody.Core.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        // "user" | "specialist" | "admin"
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "user";

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation
        public virtual ICollection<UserInjuryProfile> InjuryProfiles { get; set; } = [];
        public virtual ICollection<DailyCheckIn> DailyCheckIns { get; set; } = [];
        public virtual ICollection<UserWorkoutSession> WorkoutSessions { get; set; } = [];
        public virtual ICollection<UserChallenge> Challenges { get; set; } = [];
        public virtual ICollection<PointTransaction> Points { get; set; } = [];
        public virtual ICollection<Notification> Notifications { get; set; } = [];
    }
}