using AutoMapper;
using Omia.BLL.DTOs.CourseDiscussion;
using Omia.BLL.Helpers;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class CourseDiscussionService : ICourseDiscussionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseDiscussionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateDiscussionResponseDTO> SendMessageAsync(CreateDiscussionMessageDTO dto, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                return new CreateDiscussionResponseDTO { IsSuccess = false, Message = "Message cannot be empty" };
            }

            var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
            if (course == null)
            {
                return new CreateDiscussionResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, dto.CourseId);
            if (!hasAccess)
            {
                return new CreateDiscussionResponseDTO { IsSuccess = false, Message = "Student not enrolled in this course" };
            }

            var discussion = _mapper.Map<CourseDiscussion>(dto);
            discussion.SenderId = userId;

            await _unitOfWork.CourseDiscussions.AddAsync(discussion);

            await DiscussionNotificationHelper.CreateDiscussionNotificationsAsync(
                _unitOfWork,
                dto.Receiver,
                dto.CourseId,
                discussion.Id,
                userId,
                course.TeacherId
            );

            await _unitOfWork.CommitAsync();

            return new CreateDiscussionResponseDTO
            {
                IsSuccess = true,
                Message = "Message posted successfully",
                DiscussionId = discussion.Id,
                CreatedAt = discussion.CreatedAt
            };
        }

        public async Task<List<GetDiscussionResponseDTO>> GetCourseDiscussionsAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);
            if (!hasAccess)
                return new List<GetDiscussionResponseDTO>();

            var discussions = await _unitOfWork.CourseDiscussions.GetDiscussionsByCourseAsync(courseId);

            return _mapper.Map<List<GetDiscussionResponseDTO>>(discussions);
        }

        public async Task<List<ChatHomeItemDTO>> GetChatHomeAsync(Guid userId)
        {
            return await _unitOfWork.CourseDiscussions.GetChatHomeItemsAsync(userId);
        }

        public async Task<List<GetDiscussionResponseDTO>> GetPrivateChatAsync(Guid currentUserId, Guid otherUserId)
        {
            var discussions = await _unitOfWork.CourseDiscussions.GetPrivateChatAsync(currentUserId, otherUserId);
            return _mapper.Map<List<GetDiscussionResponseDTO>>(discussions);
        }
    }
}
