namespace VetiCare.Domain.Entities;

public class Breed : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
