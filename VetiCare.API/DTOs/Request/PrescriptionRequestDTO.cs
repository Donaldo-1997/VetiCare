namespace VetiCare.API.DTOs.Request
{
    public class PrescriptionRequestDTO
    {
        public int Quantity { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int MedicalRecordId { get; set; }
        public int MedicineId { get; set; }
    }
}
