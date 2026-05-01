using Microsoft.EntityFrameworkCore;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;

namespace Omia.BLL.Repositories.Implementation
{
    public class InstituteRepository : GenericRepository<Institute>, IInstituteRepository
    {
        public InstituteRepository(OmiaDbContext context) : base(context) { }

        public async Task<IEnumerable<Institute>> GetAllWithDetailsAsync()
        {
            return await _context.Institutes
                .Include(i => i.RegisteredStudents)
                .Include(i => i.Courses)
                .Where(i => !i.IsDeleted)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<Institute?> GetWithDetailsByIdAsync(Guid id)
        {
            return await _context.Institutes
                .Include(i => i.RegisteredStudents)
                .Include(i => i.Courses).ThenInclude(c => c.CourseStudents)
                .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);
        }

        public async Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null)
        {
            var match = await _context.Institutes.FirstOrDefaultAsync(
                i => i.Username == username && !i.IsDeleted && (excludeId == null || i.Id != excludeId));
            return match != null;
        }
    }
}
