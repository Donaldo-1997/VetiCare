using VetiCare.Domain.Enums;

namespace VetiCare.Domain.Entities;

public class Appointment : AuditBase
{
    public DateTime ScheduledAt { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string Reason { get; set; } = string.Empty;

    // Foreign Keys
    public int PetId { get; set; }
    public int VetId { get; set; }
    public int MedicalRecordId { get; set; }

    // Navigation Properties
    public Pet Pet { get; set; } = null!;
    public Vet Vet { get; set; } = null!;
    public MedicalRecord MedicalRecord { get; set; } = null!;
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
