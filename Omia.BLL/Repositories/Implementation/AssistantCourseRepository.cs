using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Omia.BLL.Repositories.Implementation
{
    public class AssistantCourseRepository : GenericRepository<AssistantCourse>, IAssistantCourseRepository
    {
        private readonly OmiaDbContext _context;
        public AssistantCourseRepository(OmiaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Guid>> GetAssistantIdsByCourseAsync(Guid courseId)
        {
            return await _context.AssistantCourses
                .Where(ac => ac.CourseId == courseId)
                .Select(ac => ac.AssistantId)
                .ToListAsync();
        }

        public async Task<bool> IsAssistantAssignedAsync(Guid assistantId, Guid courseId)
        {
            return await _context.AssistantCourses
                .AnyAsync(ac => ac.AssistantId == assistantId && ac.CourseId == courseId);
        }

        public async Task<AssistantCourse?> GetAssignmentAsync(Guid assistantId, Guid courseId)
        {
            return await _context.AssistantCourses
                .FirstOrDefaultAsync(ac => ac.AssistantId == assistantId && ac.CourseId == courseId);
        }
    }
}
