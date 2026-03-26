using CtrlZMyBody.Core.Models;

namespace CtrlZMyBody.API.Models
{
    public class SpecialistDashboardViewModel
    {
        public List<Consultation> PendingConsultations { get; set; } = [];
        public List<Consultation> MyRecentConsultations { get; set; } = [];
    }
}
