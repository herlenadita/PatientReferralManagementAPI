using AutoMapper;
using PatientReferralManagementAPI.Models;
using PatientReferralManagementAPI.DTO.Patient;
namespace PatientReferralManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientResponseDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
