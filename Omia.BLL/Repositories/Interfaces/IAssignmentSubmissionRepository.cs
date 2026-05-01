using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.Repositories.Interfaces.Base;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IAssignmentSubmissionRepository : IGenericRepository<AssignmentSubmission> {

        Task<IEnumerable<AssignmentSubmission>> GetMySubmissionsAsync(Guid assignmentId, Guid studentId);
        Task<IEnumerable<AssignmentSubmission>> GetAllSubmissionsByAssignmentAsync(Guid assignmentId);
    }
}
