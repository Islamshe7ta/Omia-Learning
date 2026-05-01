using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Course;
using Omia.BLL.DTOs.LiveSession;
using Omia.BLL.Helpers;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Services.Implementation
{
    public class LiveSessionService : ILiveSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LiveSessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLiveSessionResponseDTO> CreateLiveSessionAsync(CreateLiveSessionDTO dto, Guid userId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
            if (course == null)
            {
                return new CreateLiveSessionResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            if (dto.CategoryId.HasValue)
            {
                 var category = await _unitOfWork.CourseCategories.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                {
                    return new CreateLiveSessionResponseDTO { IsSuccess = false, Message = "Category not found" };
                }
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork,
                userId,
                dto.CourseId,
                AssistantPermissions.ManageLiveSessions
            );

            if (!hasPermission)
            {
                return new CreateLiveSessionResponseDTO { IsSuccess = false, Message = "You are not allowed to create live sessions" };
            }

            var liveSession = _mapper.Map<LiveSession>(dto);
            liveSession.CreatedBy = userId;

            await _unitOfWork.LiveSessions.AddAsync(liveSession);
            await _unitOfWork.CommitAsync();

            return new CreateLiveSessionResponseDTO
            {
                IsSuccess = true,
                Message = "Live session created successfully",
                LiveSessionId = liveSession.Id,
                CreatedAt = liveSession.CreatedAt
            };
        }
        public async Task<BaseResponseDTO> UpdateLiveSessionAsync(Guid liveSessionId, UpdateLiveSessionDTO dto, Guid userId)
        {
            var liveSession = await _unitOfWork.LiveSessions.GetByIdAsync(liveSessionId);
            if (liveSession == null)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "Live session not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork,
                userId,
                liveSession.CourseId,
                AssistantPermissions.ManageLiveSessions
            );

            if (!hasPermission)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to update live sessions" };
            }

            _mapper.Map(dto, liveSession);
            _unitOfWork.LiveSessions.Update(liveSession);

            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Live session updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteLiveSessionAsync(Guid liveSessionId, Guid userId)
        {
            var liveSession = await _unitOfWork.LiveSessions.GetByIdAsync(liveSessionId);
            if (liveSession == null)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "Live session not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork,
                userId,
                liveSession.CourseId,
                AssistantPermissions.ManageLiveSessions
            );

            if (!hasPermission)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to delete live sessions" };
            }

            _unitOfWork.LiveSessions.Delete(liveSession);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Live session deleted successfully" };
        }

        public async Task<IEnumerable<GetLiveSessionResponseDTO>> GetLiveSessionsByCourseAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);
            if (!hasAccess)
            {
                return Enumerable.Empty<GetLiveSessionResponseDTO>();
            }

            var liveSessions = await _unitOfWork.LiveSessions.GetLiveSessionsByCourseId(courseId);
            if (liveSessions == null || !liveSessions.Any())
            {
                return Enumerable.Empty<GetLiveSessionResponseDTO>();
            }

            var mappedSessions = _mapper.Map<IEnumerable<GetLiveSessionResponseDTO>>(liveSessions);

            return mappedSessions;
        }


    }
}
