using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;
using Omia.BLL.DTOs.CourseContent;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ICourseContentRepository : IGenericRepository<CourseContent>
    {
        Task<int> GetContentCountByTypeAsync(Guid courseId, ContentType contentType);
        Task<IEnumerable<CourseContent>> GetNewestVideosAsync(Guid courseId);
        Task<IEnumerable<CourseContent>> GetCourseContentsAsync(Guid courseId);
        Task<IEnumerable<CourseContent>> GetCategoryContentsAsync(Guid categoryId);
    }
}
