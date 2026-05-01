using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseContentCommentRepository : IGenericRepository<CourseContentComment> 
    {
        Task<IEnumerable<CourseContentComment>> GetCommentsWithSendersAsync(Guid contentId);
        Task<bool> IsTeacherInCourseAsync(Guid TeacherId, Guid courseId);
        Task<bool> IsAssistantInCourseAsync(Guid AssistantId, Guid courseId);
    

    }
}
