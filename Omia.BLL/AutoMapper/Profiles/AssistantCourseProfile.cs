using AutoMapper;
using Omia.BLL.DTOs.AssistantCourse;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class AssistantCourseProfile : Profile
    {
        public AssistantCourseProfile()
        {
            CreateMap<AssignAssistantToCourseDTO, AssistantCourse>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => (AssistantPermissions)src.Permissions));

            CreateMap<UpdateAssistantRolesDTO, AssistantCourse>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => (AssistantPermissions)src.Permissions));
        }
    }
}
