namespace VetiCare.API.DTOs.Request
{
    public class MedicalRecordRequestDTO
    {
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public int PetId { get; set; }
        public int AppointmentId { get; set; }


    }
}
