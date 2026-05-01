using AutoMapper;
using Omia.BLL.DTOs.CourseDiscussion;
using Omia.BLL.Helpers;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class CourseDiscussionProfile : Profile
    {
        public CourseDiscussionProfile()
        {
            CreateMap<CreateDiscussionMessageDTO, CourseDiscussion>()
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => MessageType.Text))
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver));

            CreateMap<Omia.DAL.Models.Base.BaseUserEntity, SenderProfileDTO>()
                .ForMember(dest => dest.Subtitle, opt => opt.MapFrom(src => UserRoleHelper.GetUserRole(src)));

            CreateMap<CourseDiscussion, GetDiscussionResponseDTO>()
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver))
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => src.MessageType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.SenderUserProfile, opt => opt.MapFrom(src => src.Sender));
        }
    }
}
