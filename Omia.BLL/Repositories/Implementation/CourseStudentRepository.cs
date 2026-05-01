using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseStudentRepository : GenericRepository<CourseStudent>, ICourseStudentRepository
    {
        private readonly OmiaDbContext _context;
        public CourseStudentRepository(OmiaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetEnrolledCoursesByStudentIdAsync(Guid studentId)
        {
            return await _context.CourseStudents
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Course).ThenInclude(c => c.Teacher)
                .Include(cs => cs.Course).ThenInclude(c => c.Institute)
                .Include(cs => cs.Course).ThenInclude(c => c.AssistantCourses).ThenInclude(ac => ac.Assistant)
                .Select(cs => cs.Course)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> IsStudentEnrolledInCourseAsync(Guid studentId, Guid courseId)
        {
            return await _context.CourseStudents
                          .AnyAsync(cs => cs.StudentId == studentId && cs.CourseId == courseId);

        }

        public async Task<List<Guid>> GetStudentIdsByCourseAsync(Guid courseId)
        {
            return await _context.CourseStudents
                .Where(cs => cs.CourseId == courseId)
                .Select(cs => cs.StudentId)
                .ToListAsync();
        }

        public async Task<CourseStudent?> GetEnrollmentAsync(Guid studentId, Guid courseId)
        {
            return await _context.CourseStudents
                .FirstOrDefaultAsync(cs => cs.StudentId == studentId && cs.CourseId == courseId);
        }
    }
}
