using Microsoft.EntityFrameworkCore;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;

namespace Omia.BLL.Repositories.Implementation
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(OmiaDbContext context) : base(context) { }

        public async Task<IEnumerable<Teacher>> GetSoloTeachersWithDetailsAsync()
        {
            return await _context.Teachers
                .Include(t => t.RegisteredStudents)
                .Include(t => t.Courses)
                .Where(t => t.InstituteId == null && !t.IsDeleted)
                .OrderBy(t => t.FullName)
                .ToListAsync();
        }

        public async Task<Teacher?> GetWithDetailsByIdAsync(Guid id)
        {
            return await _context.Teachers
                .Include(t => t.RegisteredStudents)
                .Include(t => t.Courses).ThenInclude(c => c.CourseStudents)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null)
        {
            var match = await _context.Teachers.FirstOrDefaultAsync(
                t => t.Username == username && !t.IsDeleted && (excludeId == null || t.Id != excludeId));
            return match != null;
        }

        public async Task<Teacher?> GetByIdAndInstituteIdAsync(Guid id, Guid? instituteId)
        {
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted && (instituteId == null || t.InstituteId == instituteId));
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersByContextAsync(Guid? instituteId)
        {
            var query = _context.Teachers.Where(t => !t.IsDeleted);
            
            if (instituteId.HasValue)
            {
                query = query.Where(t => t.InstituteId == instituteId.Value);
            }
            return await query.OrderBy(t => t.FullName).ToListAsync();
        }
    }
}
