using AutoMapper;
using Omia.BLL.DTOs.Assistant;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class AssistantProfile : Profile
    {
        public AssistantProfile()
        {
            CreateMap<CreateAssistantDTO, Assistant>();
            
            CreateMap<EditAssistantDTO, Assistant>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());
        }
    }
}
