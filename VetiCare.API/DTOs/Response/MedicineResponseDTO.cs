namespace VetiCare.API.DTOs.Response
{
    public class MedicineResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
