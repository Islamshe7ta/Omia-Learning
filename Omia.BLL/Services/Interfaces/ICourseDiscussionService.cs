using Omia.BLL.DTOs.CourseDiscussion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ICourseDiscussionService
    {
        Task<CreateDiscussionResponseDTO> SendMessageAsync(CreateDiscussionMessageDTO dto, Guid userId);
        Task<List<GetDiscussionResponseDTO>> GetCourseDiscussionsAsync(Guid courseId, Guid userId);
        Task<List<ChatHomeItemDTO>> GetChatHomeAsync(Guid userId);
        Task<List<GetDiscussionResponseDTO>> GetPrivateChatAsync(Guid currentUserId, Guid otherUserId);
    }
}
