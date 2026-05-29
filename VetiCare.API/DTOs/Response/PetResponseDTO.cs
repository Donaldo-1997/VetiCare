using VetiCare.Domain.Enums;

namespace VetiCare.API.DTOs.Response
{
    public class PetResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public float Weight { get; set; }
        public PetGender Gender { get; set; }

        public string BreedName { get; set; } = null!;
        public string OwnerName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
