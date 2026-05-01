using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseEnrollment;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ICourseEnrollmentService
    {
        Task<EnrollmentResponseDTO> AssignStudentToCourseAsync(AssignStudentToCourseDTO request);
        Task<BaseResponseDTO> RemoveStudentFromCourseAsync(RemoveStudentFromCourseDTO request);
    }
}
