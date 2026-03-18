using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IChallengeRepository : IRepository<Challenge>
    {
        Task<IEnumerable<Challenge>> GetActiveAsync();
        Task<IEnumerable<UserChallenge>> GetUserChallengesAsync(int userId);
        Task<UserChallenge?> GetUserChallengeAsync(int userId, int challengeId);
        Task<IEnumerable<UserChallenge>> GetLeaderboardAsync(int challengeId, int top = 10);
        Task<UserChallenge> AddUserChallengeAsync(UserChallenge uc);
        Task UpdateUserChallengeAsync(UserChallenge uc);
    }
}