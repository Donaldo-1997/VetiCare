namespace VetiCare.Domain.Entities;
public class Owner : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
