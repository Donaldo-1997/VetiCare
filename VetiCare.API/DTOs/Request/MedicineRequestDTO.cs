namespace VetiCare.API.DTOs.Request
{
    public class MedicineRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;

    }
}
