using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
    {
        Task<WorkoutPlan?> GetPlanWithDaysAsync(int planId);
        Task<WorkoutPlan?> GetByInjuryAndDifficultyAsync(int injuryTypeId, int difficultyLevelId);
        Task<WorkoutPlanDay?> GetDayWithExercisesAsync(int planDayId);
    }
}