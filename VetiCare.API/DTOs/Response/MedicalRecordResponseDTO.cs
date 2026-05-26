namespace VetiCare.API.DTOs.Response
{
    public class MedicalRecordResponseDTO
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int PetId { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string VetName { get; set; } = string.Empty;
        public  DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
