using CtrlZMyBody.Core.Models;
using CtrlZMyBody.Repository.Interfaces;
using CtrlZMyBody.Services.Interfaces;

namespace CtrlZMyBody.Services.Implementation
{
    public class ConsultationService : IConsultationService
    {
        private readonly IConsultationRepository _repo;

        public ConsultationService(IConsultationRepository repo) => _repo = repo;

        public async Task<Consultation> CreateAsync(int userId, string problem, string? photoUrl) =>
            await _repo.AddAsync(new Consultation
            {
                UserId = userId,
                ProblemDescription = problem,
                PhotoUrl = photoUrl,
                Status = "pending",
                RequestedAt = DateTime.UtcNow
            });

        public async Task<IEnumerable<Consultation>> GetByUserAsync(int userId) =>
            await _repo.GetByUserIdAsync(userId);

        public async Task<IEnumerable<Consultation>> GetPendingAsync() =>
            await _repo.GetPendingAsync();

        public async Task<Consultation> RespondAsync(int consultationId, int specialistId, string response)
        {
            var consultation = await _repo.GetByIdAsync(consultationId)
                ?? throw new Exception("Консультацію не знайдено.");

            consultation.SpecialistId = specialistId;
            consultation.Status = "completed";
            consultation.ResponseText = response;
            consultation.RespondedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(consultation);
            return consultation;
        }
    }
}