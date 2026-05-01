using Microsoft.EntityFrameworkCore;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.Repositories.Interfaces.Base;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IAssignmentRepository : IGenericRepository<Assignment> {
        Task<IEnumerable<Assignment>> GetAssignmentsByCourseId(Guid courseId, Guid userId);
    }
}
