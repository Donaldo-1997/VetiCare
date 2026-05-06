namespace VetiCare.Domain.Entities;
public class Medicine : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string ActiveIngredient { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
