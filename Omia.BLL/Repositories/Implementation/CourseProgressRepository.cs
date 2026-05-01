using Microsoft.EntityFrameworkCore;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseProgressRepository : GenericRepository<CourseProgress>, ICourseProgressRepository
    {
        private readonly OmiaDbContext _context;

        public CourseProgressRepository(OmiaDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<CourseProgress?> GetStudentProgressForContentAsync(Guid studentId, Guid contentId)
        {
            return await _context.CourseProgresses
                .FirstOrDefaultAsync(cp => cp.StudentId == studentId && cp.ContentId == contentId);
        }

        public async Task<IEnumerable<CourseProgress>> GetStudentProgressesForCourseAsync(Guid studentId, Guid courseId)
        {
            return await _context.CourseProgresses
                .Where(cp => cp.StudentId == studentId && cp.CourseId == courseId)
                .ToListAsync();
        }
    }
}
