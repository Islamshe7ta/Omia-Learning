using AutoMapper;
using Microsoft.IdentityModel.Tokens.Experimental;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseContent;
using Omia.BLL.Helpers;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Services.Implementation
{
    public class CourseContentService : ICourseContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CourseContentService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<CreateCourseContentResponseDTO> CreateCourseContentAsync(CreateCourseContentDTO createCourseContentDTO, Guid uploaderId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(createCourseContentDTO.CourseId);
            if (course == null)
            {
                return new CreateCourseContentResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            if (createCourseContentDTO.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CourseCategories.GetByIdAsync(createCourseContentDTO.CategoryId.Value);
                if (category == null || category.CourseId != course.Id)
                {
                    return new CreateCourseContentResponseDTO { IsSuccess = false, Message = "Category not found" };
                }
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, uploaderId, course.Id);
            if (!hasPermission)
            {
                return new CreateCourseContentResponseDTO
                {
                    IsSuccess = false,
                    Message = "You are not allowed to create content"
                };
            }

            var courseContent = _mapper.Map<CourseContent>(createCourseContentDTO);
            courseContent.UploadedBy = uploaderId;

            if (!createCourseContentDTO.OrderNumber.HasValue)
            {
                var maxOrder = await _unitOfWork.CourseContents.MaxAsync(c => c.CategoryId == createCourseContentDTO.CategoryId && c.CourseId == createCourseContentDTO.CourseId, c => c.OrderNumber);
                courseContent.OrderNumber = maxOrder + 1;
            }

            if (createCourseContentDTO.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(createCourseContentDTO.File, "contents");
                if (uploadResult.IsSuccess)
                {
                    courseContent.Url = uploadResult.FileUrl;
                }
            }

            await _unitOfWork.CourseContents.AddAsync(courseContent);
            await _unitOfWork.CommitAsync();

            return new CreateCourseContentResponseDTO
            {
                IsSuccess = true,
                Message = "Course content created successfully",
                ContentId = courseContent.Id,
                CreatedAt = courseContent.CreatedAt
            };
        }


        public async Task<BaseResponseDTO> UpdateCourseContentAsync(UpdateCourseContentDTO updateCourseContentDTO, Guid courseContentId, Guid userId)
        {
            var courseContent = await _unitOfWork.CourseContents.GetByIdAsync(courseContentId);
            if (courseContent == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Course content not found" };

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, userId, courseContent.CourseId);
            if (!hasPermission)
                return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to edit content" };

            if (updateCourseContentDTO.CategoryId != courseContent.CategoryId && updateCourseContentDTO.CategoryId.HasValue)
            {
                var newCategory = await _unitOfWork.CourseCategories.GetByIdAsync(updateCourseContentDTO.CategoryId.Value);
                if (newCategory == null || newCategory.CourseId != courseContent.CourseId)
                    return new BaseResponseDTO { IsSuccess = false, Message = "Category not found" };
            }

            var originalOrder = courseContent.OrderNumber;
            _mapper.Map(updateCourseContentDTO, courseContent);

            if (!updateCourseContentDTO.OrderNumber.HasValue)
            {
                courseContent.OrderNumber = originalOrder;
            }

            if (updateCourseContentDTO.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(updateCourseContentDTO.File, "contents");
                if (uploadResult.IsSuccess)
                {
                    courseContent.Url = uploadResult.FileUrl;
                }
            }

            _unitOfWork.CourseContents.Update(courseContent);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Course content updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteCourseContentAsync(Guid courseContentId, Guid userId)
        {
            var courseContent = await _unitOfWork.CourseContents.GetByIdAsync(courseContentId);
            if (courseContent == null)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "Course content not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, userId, courseContent.CourseId);
            if (!hasPermission)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to delete content" };
            }

            _unitOfWork.CourseContents.Delete(courseContent);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Course content deleted successfully" };
        }



        public async Task<CourseContentDetailsDTO> GetCourseContentDetailsAsync(Guid contentId, Guid userId)
        {
            var courseContent = await _unitOfWork.CourseContents.GetByIdAsync(contentId);
            if (courseContent == null)
                return new CourseContentDetailsDTO { IsSuccess = false, Message = "Content not found" };

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseContent.CourseId);
            if (!hasAccess)
                return new CourseContentDetailsDTO { IsSuccess = false, Message = "You are not allowed to view this content" };

            var studentProgress = await GetStudentProgressAsync(userId, contentId);
            var comments = await GetContentCommentsAsync(contentId);
            var contentDetails = _mapper.Map<ContentDetailsDTO>(courseContent);
            contentDetails.StudentCourseProgress = studentProgress;
            contentDetails.Comments = comments;

            return new CourseContentDetailsDTO
            {
                IsSuccess = true,
                Message = "Content retrieved successfully",
                Content = contentDetails
            };
        }

        public async Task<CommentResponseDTO> AddCommentAsync(AddContentCommentDTO request, Guid userId)
        {
            var courseContent = await _unitOfWork.CourseContents.GetByIdAsync(request.CourseContentId);
            if (courseContent == null)
                return new CommentResponseDTO { IsSuccess = false, Message = "Content not found" };

            if (string.IsNullOrWhiteSpace(request.Message))
                return new CommentResponseDTO { IsSuccess = false, Message = "Message is required" };

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseContent.CourseId);
            if (!hasAccess)
                return new CommentResponseDTO { IsSuccess = false, Message = "You are not allowed to comment on this content" };

            var comment = _mapper.Map<CourseContentComment>(request);
            comment.SenderId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.CourseContentComments.AddAsync(comment);
            await _unitOfWork.CommitAsync();

            return new CommentResponseDTO
            {
                IsSuccess = true,
                Message = "Comment added successfully",
                CommentId = comment.Id,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<ContentCountDTO> GetCourseContentCountAsync(Guid courseId, Guid userId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                return new ContentCountDTO { IsSuccess = false, Message = "Course not found" };

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);
            if (!hasAccess)
                return new ContentCountDTO { IsSuccess = false, Message = "You are not allowed to access this course" };

            var filesCount = await _unitOfWork.CourseContents.GetContentCountByTypeAsync(courseId,ContentType.File);
            var videosCount = await _unitOfWork.CourseContents.GetContentCountByTypeAsync(courseId, ContentType.Video);
            var PdfCount = await _unitOfWork.CourseContents.GetContentCountByTypeAsync(courseId, ContentType.PDF);
            var LiveSessionsCount = await _unitOfWork.CourseContents.GetContentCountByTypeAsync(courseId, ContentType.LiveSession);

            return new ContentCountDTO
            {
                IsSuccess = true,
                Message = "Counts retrieved successfully",
                FileCount = filesCount,
                VideoCount = videosCount,
                PdfCount = PdfCount,
                LiveSessionCount = LiveSessionsCount
            };
        }

        public async Task<IEnumerable<VideoSummaryDTO>?> GetNewestVideosAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);
            if (!hasAccess)
                return null;

            var newestVideos = await _unitOfWork.CourseContents.GetNewestVideosAsync(courseId);
            return _mapper.Map<IEnumerable<VideoSummaryDTO>>(newestVideos);
        }

        public async Task<IEnumerable<ContentDetailsDTO>> GetCourseContentsAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);
            if (!hasAccess)
                return null;

            var contents = await _unitOfWork.CourseContents.GetCourseContentsAsync(courseId);
            var contentDetailsList = _mapper.Map<IEnumerable<ContentDetailsDTO>>(contents).ToList();

            var student = await _unitOfWork.Students.GetByIdAsync(userId);
            if (student != null)
            {
                var progresses = await _unitOfWork.CourseProgresses
                    .GetStudentProgressesForCourseAsync(userId, courseId);

                foreach (var item in contentDetailsList)
                {
                    var prog = progresses.FirstOrDefault(p => p.ContentId == item.Id);
                    if (prog != null)
                    {
                        item.StudentCourseProgress = _mapper.Map<StudentCourseProgressDTO>(prog);
                    }
                }
            }

            return contentDetailsList;
        }

        public async Task<IEnumerable<ContentDetailsDTO>> GetCategoryContentsAsync(Guid categoryId, Guid userId)
        {
            var category = await _unitOfWork.CourseCategories.GetByIdAsync(categoryId);
            if (category == null)
                return null;

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, category.CourseId);
            if (!hasAccess)
                return null;

            var contents = await _unitOfWork.CourseContents.GetCategoryContentsAsync(categoryId);
            var contentDetailsList = _mapper.Map<IEnumerable<ContentDetailsDTO>>(contents).ToList();

            var student = await _unitOfWork.Students.GetByIdAsync(userId);
            if (student != null)
            {
                var progresses = await _unitOfWork.CourseProgresses
                    .GetStudentProgressesForCourseAsync(userId, category.CourseId);

                foreach (var item in contentDetailsList)
                {
                    var prog = progresses.FirstOrDefault(p => p.ContentId == item.Id);
                    if (prog != null)
                    {
                        item.StudentCourseProgress = _mapper.Map<StudentCourseProgressDTO>(prog);
                    }
                }
            }

            return contentDetailsList;
        }

        private async Task<StudentCourseProgressDTO?> GetStudentProgressAsync(Guid studentId, Guid contentId)
        {
            var progress = await _unitOfWork.CourseProgresses
                .GetStudentProgressForContentAsync(studentId, contentId);

            if (progress == null)
                return null;

            return _mapper.Map<StudentCourseProgressDTO>(progress);
        }

        private async Task<List<CourseCommentDTO>> GetContentCommentsAsync(Guid contentId)
        {
            var comments = await _unitOfWork.CourseContentComments
                .GetCommentsWithSendersAsync(contentId);

            var commentDTOs = new List<CourseCommentDTO>();

            foreach (var comment in comments)
            {
                var commentDTO = _mapper.Map<CourseCommentDTO>(comment);

                if (comment.Sender != null)
                {
                    commentDTO.SenderUserProfile = _mapper.Map<SenderUserProfileDTO>(comment.Sender);
                    commentDTO.SenderUserProfile.Subtitle = GetUserSubtitle(comment.Sender);
                }

                commentDTOs.Add(commentDTO);
            }

            return commentDTOs;
        }

        private string GetUserSubtitle(BaseUserEntity? user)
        {
            if (user == null) return "Unknown";

            return user switch
            {
                Teacher => "Teacher",
                Assistant => "Assistant",
                Student => "Student",
                Parent => "Parent",
                Admin => "Admin",
                _ => "User"
            };
        }


    }
}
