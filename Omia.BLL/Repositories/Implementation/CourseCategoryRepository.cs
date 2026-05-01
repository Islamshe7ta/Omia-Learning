using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseCategoryRepository : GenericRepository<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(OmiaDbContext context) : base(context) { }

        public async Task<IEnumerable<CourseCategory>> GetCategoriesByCourseId(Guid courseId)
        {
           
            return await _context.CourseCategories
                .Where(x => x.CourseId == courseId)
                .ToListAsync();
        }


        public async Task<int> GetCategoryCountByCourseId(Guid courseId)
        {
           return await _context.CourseCategories.Where(x => x.CourseId == courseId).CountAsync();
        }

        public async Task<bool> IsCategoryInCourseAsync(Guid categoryId, Guid courseId)
        {
            return await _context.CourseCategories.AnyAsync(x => x.Id == categoryId && x.CourseId == courseId);
        }
    }
}
