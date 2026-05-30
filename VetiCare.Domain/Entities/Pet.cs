using VetiCare.Domain.Enums;

namespace VetiCare.Domain.Entities;

public class Pet : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public float Weight { get; set; }
    public PetGender Gender { get; set; }

    // Foreign Keys
    public int OwnerId { get; set; }
    public int BreedId { get; set; }

    // Navigation Properties
    public Owner Owner { get; set; } = null!;
    public Breed Breed { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
