namespace VetiCare.API.DTOs.Response
{
    public class AppointmentResponseDTO
    {
        public int Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;

        // Datos desnormalizados de Pet y Vet para no exponer entidades completas
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public int VetId { get; set; }
        public string VetName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
