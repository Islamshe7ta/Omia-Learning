using Omia.BLL.Repositories.Interfaces.Base;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseCategoryRepository : IGenericRepository<CourseCategory>
    {

        Task<IEnumerable<CourseCategory>> GetCategoriesByCourseId(Guid courseId);

        Task<int> GetCategoryCountByCourseId(Guid courseId);
        Task<bool> IsCategoryInCourseAsync(Guid categoryId, Guid courseId);
    }
}
