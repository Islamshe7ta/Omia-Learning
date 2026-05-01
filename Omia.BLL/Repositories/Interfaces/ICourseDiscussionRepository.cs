using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;
using Omia.BLL.DTOs.CourseDiscussion;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseDiscussionRepository : IGenericRepository<CourseDiscussion>
    {
        Task<List<CourseDiscussion>> GetDiscussionsByCourseAsync(Guid courseId);
        Task<List<ChatHomeItemDTO>> GetChatHomeItemsAsync(Guid userId);
        Task<List<CourseDiscussion>> GetPrivateChatAsync(Guid currentUserId, Guid otherUserId);
    }
}
