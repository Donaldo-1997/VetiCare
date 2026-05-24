namespace VetiCare.API.DTOs.Response
{
    public class PrescriptionResponseDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int MedicalRecordId { get; set; }
        public string MedicalRecordName { get; set; } = string.Empty;
        public int MedicineId { get; set; }
        public string MedicineName { get;  set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
