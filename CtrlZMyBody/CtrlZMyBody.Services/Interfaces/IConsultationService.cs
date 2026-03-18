using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Services.Interfaces
{
    public interface IConsultationService
    {
        Task<Consultation> CreateAsync(int userId, string problem, string? photoUrl);
        Task<IEnumerable<Consultation>> GetByUserAsync(int userId);
        Task<IEnumerable<Consultation>> GetPendingAsync();
        Task<Consultation> RespondAsync(int consultationId, int specialistId, string response);
    }
}