using AutoMapper;
using Omia.BLL.DTOs.Profile;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UpdateProfileDTO, BaseUserEntity>()
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.FullName)))
                .ForMember(dest => dest.Username, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Username)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null));
            
            // Individual profile update mappings
            CreateMap<UpdateFullnameDTO, BaseUserEntity>();
            CreateMap<UpdateUsernameDTO, BaseUserEntity>();
            CreateMap<UpdateEmailDTO, BaseUserEntity>();
            CreateMap<UpdatePhoneNumberDTO, BaseUserEntity>();

            // Location mappings
            CreateMap<UpdateLocationDTO, BaseUserEntity>();

            CreateMap<UpdateLocationDTO, Student>()
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Location));
                
            CreateMap<UpdateLocationDTO, Teacher>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location));

            CreateMap<UpdateLocationDTO, Institute>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location));
            
            CreateMap<BaseUserEntity, ProfileDTO>()
                .Include<Student, ProfileDTO>()
                .Include<Teacher, ProfileDTO>()
                .Include<Institute, ProfileDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => string.Empty))
                .ForMember(dest => dest.EducationStage, opt => opt.Ignore())
                .ForMember(dest => dest.Governorate, opt => opt.Ignore());

            CreateMap<Student, ProfileDTO>()
                .IncludeBase<BaseUserEntity, ProfileDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Governorate))
                .ForMember(dest => dest.EducationStage, opt => opt.MapFrom(src => src.EducationStage))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate));

            CreateMap<Teacher, ProfileDTO>()
                .IncludeBase<BaseUserEntity, ProfileDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address));

            CreateMap<Institute, ProfileDTO>()
                .IncludeBase<BaseUserEntity, ProfileDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Address));
        }
    }
}
