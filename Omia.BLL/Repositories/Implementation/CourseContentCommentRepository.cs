using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseContentCommentRepository : GenericRepository<CourseContentComment>, ICourseContentCommentRepository
    {
        private readonly OmiaDbContext _context;

        public CourseContentCommentRepository(OmiaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseContentComment>> GetCommentsWithSendersAsync(Guid contentId)
        {
            return await _context.CourseContentComments
                .Where(cc => cc.CourseContentId == contentId)
                .Include(cc => cc.Sender)
                .OrderBy(cc => cc.CreatedAt)
                .ToListAsync();
        }
        public Task<bool> IsTeacherInCourseAsync(Guid TeacherId, Guid courseId)
        {
            return _context.Courses.AnyAsync(cs => cs.TeacherId == TeacherId && cs.Id == courseId);
        }
        public Task<bool> IsAssistantInCourseAsync(Guid AssistantId, Guid courseId)
        {
            return _context.AssistantCourses.AnyAsync(cs => cs.AssistantId == AssistantId && cs.CourseId == courseId);
        }


    }
}
