using Microsoft.EntityFrameworkCore;
using Omia.BLL.DTOs.CourseContent;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseContentRepository : GenericRepository<CourseContent>, ICourseContentRepository
    {
        private readonly OmiaDbContext _context;
        public CourseContentRepository(OmiaDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<int> GetContentCountByTypeAsync(Guid courseId, ContentType contentType) 
        {
            var count = await _context.CourseContents.CountAsync(cc =>
                cc.CourseId == courseId &&
                cc.ContentType == contentType &&
                !cc.IsDeleted);

            return count;
        }

        public async Task<IEnumerable<CourseContent>> GetNewestVideosAsync(Guid courseId)
        {
            return await _context.CourseContents
                .Where(cc => cc.CourseId == courseId && cc.ContentType == ContentType.Video && !cc.IsDeleted)
                .OrderByDescending(cc => cc.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseContent>> GetCourseContentsAsync(Guid courseId)
        {
            return await _context.CourseContents
                .Where(cc => cc.CourseId == courseId && !cc.IsDeleted)
                .OrderBy(cc => cc.OrderNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseContent>> GetCategoryContentsAsync(Guid categoryId)
        {
            return await _context.CourseContents
                .Where(cc => cc.CategoryId == categoryId && !cc.IsDeleted)
                .OrderBy(cc => cc.OrderNumber)
                .ToListAsync();
        }
    }
}
