using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student> 
    {
        Task<Student?> GetByIdAndContextAsync(Guid id, Guid? teacherId, Guid? instituteId);
        Task<IEnumerable<Student>> GetAllByContextAsync(Guid? teacherId, Guid? instituteId);
        Task<IEnumerable<Student>> GetByCourseIdAsync(Guid courseId);
    }
}
