using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Course;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        
        public async Task<CourseCreateResponseDTO> CreateCourseAsync(CreateCourseDTO courseDto)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(courseDto.TeacherId.Value);
            if (teacher == null)
            {
                return new CourseCreateResponseDTO { IsSuccess = false, Message = "Teacher not found" };
            }

            var course = _mapper.Map<Course>(courseDto);

            if (courseDto.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(courseDto.File, "courses");
                if (uploadResult.IsSuccess)
                {
                    course.Image = uploadResult.FileUrl;
                }
            }

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.CommitAsync();
            return new CourseCreateResponseDTO { IsSuccess = true, Message = "Course created successfully", CourseId = course.Id };
        }

        public async Task<BaseResponseDTO> UpdateCourseAsync(Guid courseId, CreateCourseDTO courseDto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null) return new BaseResponseDTO { IsSuccess = false, Message = "Course not found" };

            var teacher = await _unitOfWork.Teachers.GetByIdAsync(courseDto.TeacherId.Value);
            if (teacher == null) return new BaseResponseDTO { IsSuccess = false, Message = "Teacher not found" };

            _mapper.Map(courseDto, course);

            if (courseDto.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(courseDto.File, "courses");
                if (uploadResult.IsSuccess)
                {
                    course.Image = uploadResult.FileUrl;
                }
            }

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Course updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteCourseAsync(Guid courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null) return new BaseResponseDTO { IsSuccess = false, Message = "Course not found" };

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Course deleted successfully" };
        }

        public async Task<CoursesResponseDTO> GetCoursesAsync(Guid userId)
        {
            var courses = await _unitOfWork.Courses.GetMyCourses(userId);

            var coursesDto = _mapper.Map<IEnumerable<StudentCourseDTO>>(courses);
            return new CoursesResponseDTO
            {
                IsSuccess = true,
                Message = "Courses retrieved successfully",
                Courses = coursesDto
            };
        }

        public async Task<CourseDetailsFullResponseDTO> GetCourseDetailsFullAsync(Guid courseId, Guid userId)
        {
            var course = await _unitOfWork.Courses.GetFullCourseDetailsAsync(courseId);
            if (course == null) return new CourseDetailsFullResponseDTO { StatusCode = 404, Message = "Course not found" };

           
            var enrollment = course.CourseStudents.FirstOrDefault(x => x.StudentId == userId);
            bool isStudent = enrollment != null;
            bool isTeacher = course.TeacherId == userId;
            bool isAssistant = course.AssistantCourses.Any(x => x.AssistantId == userId);

            if (!isStudent && !isTeacher && !isAssistant)
            {
                return new CourseDetailsFullResponseDTO { IsSuccess = false, StatusCode = 403, Message = "User not authorized to view this course" };
            }

            var courseDetails = _mapper.Map<StudentCourseDTO>(course);
            if (isStudent)
            {
                courseDetails.EnrollmentDate = enrollment.CreatedAt;
            }

            var fullDetails = new CourseDetailsFullDTO
            {
                CourseDetails = courseDetails,
                Categories = _mapper.Map<IEnumerable<CourseCategoryDTO>>(course.Categories),
                Contents = _mapper.Map<IEnumerable<CourseContentDTO>>(course.Contents),
                LiveSessions = _mapper.Map<IEnumerable<LiveSessionDTO>>(course.LiveSessions),
                Assignments = _mapper.Map<IEnumerable<AssignmentDTO>>(course.Assignments),
                Quizzes = _mapper.Map<IEnumerable<QuizDTO>>(course.Quizzes),
                Discussions = _mapper.Map<IEnumerable<CourseDiscussionDTO>>(course.Discussions)
            };

            return new CourseDetailsFullResponseDTO
            {
                IsSuccess = true,
                Message = "Course details retrieved successfully",
                StatusCode = 200,
                Data = fullDetails
            };
        }

        public async Task<CourseBriefResponseDTO> GetCourseBriefAsync(Guid courseId)
        {
            var course = await _unitOfWork.Courses.GetCourseBriefAsync(courseId);
            if (course == null)
            {
                return new CourseBriefResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            var briefDto = _mapper.Map<CourseBriefDTO>(course);
            return new CourseBriefResponseDTO
            {
                IsSuccess = true,
                Message = "Course brief retrieved successfully",
                Data = briefDto
            };
        }

        public async Task<IEnumerable<NotificationDTO>> GetActivityFeedAsync(Guid courseId, Guid studentId)
        {
            var enrollment = await _unitOfWork.CourseStudents.FirstOrDefaultAsync(x => x.CourseId == courseId && x.StudentId == studentId);
            if (enrollment == null) return Enumerable.Empty<NotificationDTO>();

            return await _unitOfWork.Courses.GetActivityFeedAsync(courseId, studentId);
        }

       

       
    }
}
