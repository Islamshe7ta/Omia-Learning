using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omia.BLL.Repositories.Implementation
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(OmiaDbContext context) : base(context) { }

        public async Task<Student?> GetByIdAndContextAsync(Guid id, Guid? teacherId, Guid? instituteId)
        {
            return await _dbSet
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted &&
                    (teacherId == null || s.TeacherId == teacherId) &&
                    (instituteId == null || s.InstituteId == instituteId));
        }

        public async Task<IEnumerable<Student>> GetAllByContextAsync(Guid? teacherId, Guid? instituteId)
        {
            var query = _dbSet.Where(s => !s.IsDeleted);

            if (teacherId.HasValue)
                query = query.Where(s => s.TeacherId == teacherId.Value);

            if (instituteId.HasValue)
                query = query.Where(s => s.InstituteId == instituteId.Value);

            return await query.OrderBy(s => s.FullName).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByCourseIdAsync(Guid courseId)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && s.CourseStudents.Any(cs => cs.CourseId == courseId))
                .OrderBy(s => s.FullName)
                .ToListAsync();
        }
    }
}
