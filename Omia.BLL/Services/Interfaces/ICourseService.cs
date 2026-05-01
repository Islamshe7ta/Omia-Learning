using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseCreateResponseDTO> CreateCourseAsync(CreateCourseDTO courseDto);
        Task<CoursesResponseDTO> GetCoursesAsync(Guid studentId);
        Task<CourseDetailsFullResponseDTO> GetCourseDetailsFullAsync(Guid courseId, Guid userId);
        Task<CourseBriefResponseDTO> GetCourseBriefAsync(Guid courseId);
        Task<IEnumerable<NotificationDTO>> GetActivityFeedAsync(Guid courseId, Guid studentId);
        Task<BaseResponseDTO> UpdateCourseAsync(Guid courseId, CreateCourseDTO courseDto);
        Task<BaseResponseDTO> DeleteCourseAsync(Guid courseId);
    }
}
