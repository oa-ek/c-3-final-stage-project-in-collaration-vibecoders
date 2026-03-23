using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class ChallengeService : IChallengeService
    {
        private readonly IChallengeRepository _repo;

        public ChallengeService(IChallengeRepository repo) => _repo = repo;

        public async Task<IEnumerable<Challenge>> GetActiveChallengesAsync() =>
            await _repo.GetActiveAsync();

        public async Task<UserChallenge> JoinChallengeAsync(int userId, int challengeId)
        {
            var existing = await _repo.GetUserChallengeAsync(userId, challengeId);
            if (existing != null)
                throw new InvalidOperationException("Ви вже берете участь у цьому челенджі.");

            return await _repo.AddUserChallengeAsync(new UserChallenge
            {
                UserId = userId,
                ChallengeId = challengeId,
                JoinedAt = DateTime.UtcNow
            });
        }

        public async Task<IEnumerable<UserChallenge>> GetUserChallengesAsync(int userId) =>
            await _repo.GetUserChallengesAsync(userId);

        public async Task<IEnumerable<UserChallenge>> GetLeaderboardAsync(int challengeId) =>
            await _repo.GetLeaderboardAsync(challengeId);

        public async Task UpdateProgressAsync(int userId, string metric, int amount)
        {
            var active = await _repo.GetActiveAsync();
            foreach (var challenge in active.Where(c => c.GoalMetric == metric))
            {
                var uc = await _repo.GetUserChallengeAsync(userId, challenge.ChallengeId);
                if (uc == null || uc.IsCompleted) continue;

                uc.CurrentValue += amount;
                if (uc.CurrentValue >= challenge.GoalValue)
                {
                    uc.IsCompleted = true;
                    uc.CompletedAt = DateTime.UtcNow;
                }
                await _repo.UpdateUserChallengeAsync(uc);
            }
        }
    }
}