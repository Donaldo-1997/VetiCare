namespace VetiCare.Domain.Entities;
public class Vet : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

