using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IAssistantCourseRepository : IGenericRepository<AssistantCourse>
    {
        Task<List<Guid>> GetAssistantIdsByCourseAsync(Guid courseId);
        Task<bool> IsAssistantAssignedAsync(Guid assistantId, Guid courseId);
        Task<AssistantCourse?> GetAssignmentAsync(Guid assistantId, Guid courseId);
    }
}
