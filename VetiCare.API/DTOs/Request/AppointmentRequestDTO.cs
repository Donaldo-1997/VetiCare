using VetiCare.Domain.Enums;

namespace VetiCare.API.DTOs.Request
{
    public class AppointmentRequestDTO
    {
        public DateTime ScheduledAt { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int PetId { get; set; }
        public int VetId { get; set; }
    }
}
