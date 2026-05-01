using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;
using Omia.BLL.DTOs.Course;


namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetMyCourses(Guid userId);
        Task<Course?> GetFullCourseDetailsAsync(Guid courseId);
        Task<Course?> GetCourseBriefAsync(Guid courseId);

        Task<IEnumerable<NotificationDTO>> GetActivityFeedAsync(Guid courseId, Guid studentId, int limit = 50);
    }
}
