using Microsoft.EntityFrameworkCore;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class AssignmentSubmissionRepository : GenericRepository<AssignmentSubmission>, IAssignmentSubmissionRepository
    {
        private readonly OmiaDbContext _context;
        public AssignmentSubmissionRepository(OmiaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssignmentSubmission>> GetMySubmissionsAsync(Guid assignmentId, Guid studentId)
        {
            return await _context.Set<AssignmentSubmission>()
                                 .Where(s => s.AssignmentId == assignmentId && s.StudentId == studentId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentSubmission>> GetAllSubmissionsByAssignmentAsync(Guid assignmentId)
        {
            return await _context.Set<AssignmentSubmission>()
                                 .Include(s => s.Student)
                                 .Where(s => s.AssignmentId == assignmentId)
                                 .ToListAsync();
        }
    }
}
