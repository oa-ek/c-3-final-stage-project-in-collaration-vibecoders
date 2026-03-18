using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class WorkoutPlanRepository : Repository<WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(AppDbContext context) : base(context) { }

        public async Task<WorkoutPlan?> GetPlanWithDaysAsync(int planId) =>
            await _dbSet
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                        .ThenInclude(e => e.Exercise)
                .Include(p => p.InjuryType)
                .Include(p => p.DifficultyLevel)
                .FirstOrDefaultAsync(p => p.PlanId == planId);

        public async Task<WorkoutPlan?> GetByInjuryAndDifficultyAsync(int injuryTypeId, int difficultyLevelId) =>
            await _dbSet
                .Include(p => p.Days)
                    .ThenInclude(d => d.Exercises)
                        .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(p =>
                    p.InjuryTypeId == injuryTypeId &&
                    p.DifficultyLevelId == difficultyLevelId &&
                    p.IsActive);

        public async Task<WorkoutPlanDay?> GetDayWithExercisesAsync(int planDayId) =>
            await _context.WorkoutPlanDays
                .Include(d => d.Exercises)
                    .ThenInclude(e => e.Exercise)
                .FirstOrDefaultAsync(d => d.PlanDayId == planDayId);
    }
}