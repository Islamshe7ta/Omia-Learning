using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly OmiaDbContext _context;

        public AssignmentRepository(OmiaDbContext context) : base(context) {
            _context = context;
        }
        public async Task<IEnumerable<Assignment>> GetAssignmentsByCourseId(Guid courseId, Guid userId)
        {
            var Assignments = await _context.Set<Assignment>()
                                         .Where(ass => ass.CourseId == courseId)
                                         .OrderBy(ass => ass.OrderNumber)
                                         .ToListAsync();

            return Assignments;
        }
    }
}
