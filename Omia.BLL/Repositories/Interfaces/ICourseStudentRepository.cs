using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseStudentRepository : IGenericRepository<CourseStudent> 
    {
        Task<IEnumerable<Course>> GetEnrolledCoursesByStudentIdAsync(Guid studentId);
        Task<bool> IsStudentEnrolledInCourseAsync(Guid studentId, Guid courseId);
        Task<List<Guid>> GetStudentIdsByCourseAsync(Guid courseId);
        Task<CourseStudent?> GetEnrollmentAsync(Guid studentId, Guid courseId);
    }
}
