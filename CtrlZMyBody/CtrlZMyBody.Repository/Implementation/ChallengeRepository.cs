using Microsoft.EntityFrameworkCore;
using CtrlZMyBody.Core.Context;
using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;

namespace CtrlZMyBody.Repository.Implementation
{
    public class ChallengeRepository : Repository<Challenge>, IChallengeRepository
    {
        public ChallengeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Challenge>> GetActiveAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _dbSet
                .Where(c => c.IsActive && c.StartDate <= today && c.EndDate >= today)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserChallenge>> GetUserChallengesAsync(int userId) =>
            await _context.UserChallenges
                .Include(uc => uc.Challenge)
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

        public async Task<UserChallenge?> GetUserChallengeAsync(int userId, int challengeId) =>
            await _context.UserChallenges
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ChallengeId == challengeId);

        public async Task<IEnumerable<UserChallenge>> GetLeaderboardAsync(int challengeId, int top = 10) =>
            await _context.UserChallenges
                .Include(uc => uc.User)
                .Where(uc => uc.ChallengeId == challengeId)
                .OrderByDescending(uc => uc.CurrentValue)
                .Take(top)
                .ToListAsync();

        public async Task<UserChallenge> AddUserChallengeAsync(UserChallenge uc)
        {
            await _context.UserChallenges.AddAsync(uc);
            await _context.SaveChangesAsync();
            return uc;
        }

        public async Task UpdateUserChallengeAsync(UserChallenge uc)
        {
            _context.UserChallenges.Update(uc);
            await _context.SaveChangesAsync();
        }
    }
}