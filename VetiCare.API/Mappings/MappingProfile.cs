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


            //Medical record mappings
            CreateMap<MedicalRecordRequestDTO, MedicalRecord>();
            CreateMap<MedicalRecord, MedicalRecordResponseDTO>()
                .ForMember(dest => dest.PetName,
                    opt => opt.MapFrom(src => src.Pet.Name))
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src => src.Pet.Owner.FirstName + " " + src.Pet.Owner.LastName))
                .ForMember(dest => dest.VetName,
                    opt => opt.MapFrom(src => src.Appointment.Vet.FirstName + " " + src.Appointment.Vet.LastName));

            //Medicine mappings
            CreateMap<MedicineRequestDTO, Medicine>();
            CreateMap<Medicine, MedicineResponseDTO>();


            //Prescription mappings
            CreateMap<PrescriptionRequestDTO, Prescription>();
            CreateMap<Prescription, PrescriptionResponseDTO>()
                .ForMember(dest => dest.MedicalRecordName,
                    opt => opt.MapFrom(src => src.MedicalRecord.Diagnosis))
                .ForMember(dest => dest.MedicineName,
                    opt => opt.MapFrom(src => src.Medicine.Name));

        }


    }
    
}
