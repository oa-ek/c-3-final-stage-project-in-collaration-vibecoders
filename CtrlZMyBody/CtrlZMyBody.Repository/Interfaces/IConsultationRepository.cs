using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.Repository.Interfaces
{
    public interface IConsultationRepository : IRepository<Consultation>
    {
        Task<IEnumerable<Consultation>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Consultation>> GetPendingAsync();
        Task<IEnumerable<Consultation>> GetBySpecialistAsync(int specialistId);
    }
}