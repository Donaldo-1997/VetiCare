using VetiCare.Domain.Enums;

namespace VetiCare.API.DTOs.Request
{
    public class PetRequestDTO
    {
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public float Weight { get; set; }
        public PetGender Gender { get; set; }
        public int OwnerId { get; set; }
        public int BreedId { get; set; }
    }
}
