using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface IChallengeService
    {
        Task<IEnumerable<Challenge>> GetActiveChallengesAsync();
        Task<UserChallenge> JoinChallengeAsync(int userId, int challengeId);
        Task<IEnumerable<UserChallenge>> GetUserChallengesAsync(int userId);
        Task<IEnumerable<UserChallenge>> GetLeaderboardAsync(int challengeId);
        Task UpdateProgressAsync(int userId, string metric, int amount);
    }
}