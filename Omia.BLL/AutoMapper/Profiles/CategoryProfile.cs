using AutoMapper;
using Omia.BLL.DTOs.CourseCategory;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            
            CreateMap<CourseCategory, CourseCategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryRequestDTO, CourseCategory>();
            CreateMap<UpdateCategoryRequestDTO, CourseCategory>();

            // Mapping for Course Statistics
            CreateMap<Course, CourseCategoryCountDTO>()
                .ForMember(dest => dest.CategoriesCount, opt => opt.MapFrom(src => src.Categories.Count))
                .ForMember(dest => dest.ContentsCount, opt => opt.MapFrom(src => src.Contents.Count))
                .ForMember(dest => dest.LiveSessionsCount, opt => opt.MapFrom(src => src.LiveSessions.Count))
                .ForMember(dest => dest.AssignmentsCount, opt => opt.MapFrom(src => src.Assignments.Count))
                .ForMember(dest => dest.QuizzesCount, opt => opt.MapFrom(src => src.Quizzes.Count))
                .ForMember(dest => dest.DiscussionsCount, opt => opt.MapFrom(src => src.Discussions.Count));
        }
    }
}
