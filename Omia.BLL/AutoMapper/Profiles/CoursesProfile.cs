using Omia.BLL.DTOs.Course;
using Omia.BLL.DTOs.CourseContent;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Omia.DAL.Models.Base;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile() 
        {
            CreateMap<Course, StudentCourseDTO>()
                .ForMember(dest => dest.TeacherUserProfile, opt => opt.MapFrom(src => src.Teacher))
                .ForMember(dest => dest.InstituteUserProfile, opt => opt.MapFrom(src => src.Institute))
                .ForMember(dest => dest.AssistantUserProfiles, opt => opt.MapFrom(src => src.AssistantCourses.Select(ac => ac.Assistant)))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.CourseStudents.Count))
                .ReverseMap();

            CreateMap<CreateCourseDTO, Course>();

            CreateMap<CreateCourseContentDTO, CourseContent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => (ContentType)src.ContentType))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.UploadedBy, opt => opt.Ignore()) 
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Uploader, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<UpdateCourseContentDTO, CourseContent>()
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => (ContentType)src.ContentType))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.UploadedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Uploader, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<CourseContent, ContentDetailsDTO>()
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => (int)src.ContentType))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId ?? Guid.Empty))
                .ForMember(dest => dest.StudentCourseProgress, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<CourseProgress, StudentCourseProgressDTO>()
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.IsCompleted ? src.CreatedAt : (DateTime?)null));

            CreateMap<CourseContentComment, CourseCommentDTO>()
                .ForMember(dest => dest.SenderUserProfile, opt => opt.Ignore());

            CreateMap<AddContentCommentDTO, CourseContentComment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.SenderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Sender, opt => opt.Ignore());

            CreateMap<BaseUserEntity, SenderUserProfileDTO>()
                .ForMember(dest => dest.Subtitle, opt => opt.Ignore()); 

            CreateMap<CourseContent, CourseContentDTO>();
            CreateMap<CourseContent, VideoSummaryDTO>();
            CreateMap<LiveSession, LiveSessionDTO>();
            CreateMap<Assignment, AssignmentDTO>();
            CreateMap<Quiz, QuizDTO>();
            CreateMap<Course, CourseBriefDTO>()
                .IncludeBase<Course, StudentCourseDTO>()
                .ForMember(dest => dest.CategoriesCount, opt => opt.MapFrom(src => src.Categories.Count))
                .ForMember(dest => dest.ContentsCount, opt => opt.MapFrom(src => src.Contents.Count))
                .ForMember(dest => dest.LiveSessionsCount, opt => opt.MapFrom(src => src.LiveSessions.Count))
                .ForMember(dest => dest.AssignmentsCount, opt => opt.MapFrom(src => src.Assignments.Count))
                .ForMember(dest => dest.QuizzesCount, opt => opt.MapFrom(src => src.Quizzes.Count))
                .ForMember(dest => dest.DiscussionsCount, opt => opt.MapFrom(src => src.Discussions.Count));

            CreateMap<CourseDiscussion, CourseDiscussionDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.FullName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<CreateCourseDTO, Course>();
        }
    }
}
