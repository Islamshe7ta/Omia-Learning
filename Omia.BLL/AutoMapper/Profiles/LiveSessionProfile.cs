using AutoMapper;
using Omia.BLL.DTOs.Course;
using Omia.BLL.DTOs.LiveSession;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class LiveSessionProfile : Profile
    {
        public LiveSessionProfile()
        {
            CreateMap<CreateLiveSessionDTO, LiveSession>();
            CreateMap<UpdateLiveSessionDTO, LiveSession>();
            CreateMap<LiveSession, GetLiveSessionResponseDTO>();
        }
    }
}
