using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.API.Models
{
    public class UserDashboardViewModel
    {
        public List<InjuryType> InjuryTypes { get; set; } = [];
        public List<DifficultyLevel> DifficultyLevels { get; set; } = [];
        public UserInjuryProfile? ActiveProfile { get; set; }
        public WorkoutPlan? CurrentPlan { get; set; }
        public WorkoutPlanDay? CurrentDay { get; set; }
        public DailyCheckIn? TodayCheckIn { get; set; }
        public List<UserWorkoutSession> RecentSessions { get; set; } = [];
        public List<Consultation> Consultations { get; set; } = [];
        public List<Challenge> ActiveChallenges { get; set; } = [];
    }
}
