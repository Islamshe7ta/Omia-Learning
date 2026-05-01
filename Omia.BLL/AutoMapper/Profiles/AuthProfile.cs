using AutoMapper;
using Omia.BLL.DTOs.Auth;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Student, StudentProfileDTO>().ReverseMap();
            CreateMap<Teacher, TeacherProfileDTO>().ReverseMap();
            CreateMap<Assistant, AssistantProfileDTO>().ReverseMap();
            CreateMap<Parent, ParentUserProfileDTO>().ReverseMap();
            CreateMap<Institute, InstituteProfileDTO>().ReverseMap();
        }
    }
}
