using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> CreateStudentAsync(CreateStudentDTO request, Guid? teacherId, Guid? instituteId);
        Task<BaseResponseDTO> UpdateStudentAsync(Guid studentId, EditStudentDTO request, Guid? teacherId, Guid? instituteId);
        Task<BaseResponseDTO> UpdateParentAsync(Guid studentId, EditParentDTO request, Guid? teacherId, Guid? instituteId);
        Task<BaseResponseDTO> DeleteStudentAsync(Guid studentId, Guid? teacherId, Guid? instituteId);
        Task<StudentProfileDTO> GetStudentByIdAsync(Guid studentId, Guid? teacherId, Guid? instituteId);
        Task<IEnumerable<StudentBriefDTO>> GetAllStudentsAsync(Guid? teacherId, Guid? instituteId);
        Task<IEnumerable<StudentBriefDTO>> GetStudentsByCourseIdAsync(Guid courseId);
    }
}
