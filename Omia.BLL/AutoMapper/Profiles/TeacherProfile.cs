using AutoMapper;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Teacher;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<CreateTeacherDTO, Teacher>();
            
            CreateMap<EditTeacherDTO, Teacher>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.InstituteId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Teacher, TeacherProfileDTO>();
        }
    }
}
