using AutoMapper;
using VetiCare.API.DTOs.Request;
using VetiCare.API.DTOs.Response;
using VetiCare.Domain.Entities;

namespace VetiCare.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Vet mappings
            CreateMap<VetRequestDTO, Vet>();
            CreateMap<Vet, VetResponseDTO>();

            // Aquí irán los mapeos de Owner, Pet, Appointment, etc.
        }
    }
}
