using AutoMapper;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Student;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentDTO, Student>();
            CreateMap<CreateStudentDTO, Parent>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ParentFullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ParentEmail))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ParentUsername))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ParentPhoneNumber))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.ParentPassword));

            CreateMap<EditStudentDTO, Student>();
            CreateMap<EditParentDTO, Parent>();

            CreateMap<Student, StudentProfileDTO>();
            CreateMap<Student, StudentBriefDTO>();
        }
    }
}
