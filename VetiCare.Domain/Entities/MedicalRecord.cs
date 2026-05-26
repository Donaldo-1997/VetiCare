namespace VetiCare.Domain.Entities;
public class MedicalRecord : AuditBase
{
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Notes { get; set; }

    // Foreign Key
    public int PetId { get; set; }
    public int AppointmentId { get; set; }

    // Navigation Properties
    public Pet Pet { get; set; } = null!;
    public Appointment Appointment { get; set; } = null!;
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
