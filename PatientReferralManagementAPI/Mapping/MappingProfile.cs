using AutoMapper;
using PatientReferralManagementAPI.DTO.Patient;
using PatientReferralManagementAPI.DTO.Referral;
using PatientReferralManagementAPI.Models;
namespace PatientReferralManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientResponseDto>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));

            CreateMap<Referral, ReferralResponseDto>();

        }
    }
}
