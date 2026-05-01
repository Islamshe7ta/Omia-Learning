using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseProgressRepository : IGenericRepository<CourseProgress> 
    {
        Task<CourseProgress?> GetStudentProgressForContentAsync(Guid studentId, Guid contentId);
        Task<IEnumerable<CourseProgress>> GetStudentProgressesForCourseAsync(Guid studentId, Guid courseId);
    }
}
