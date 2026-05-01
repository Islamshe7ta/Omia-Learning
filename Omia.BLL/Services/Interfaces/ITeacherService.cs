using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherCreatedResponseDTO> CreateTeacherAsync(CreateTeacherDTO request, Guid? instituteId);
        Task<BaseResponseDTO> UpdateTeacherAsync(Guid teacherId, EditTeacherDTO request, Guid? instituteId);
        Task<BaseResponseDTO> DeleteTeacherAsync(Guid teacherId, Guid? instituteId);
        Task<TeacherProfileDTO> GetTeacherByIdAsync(Guid teacherId, Guid? instituteId);
        Task<IEnumerable<TeacherProfileDTO>> GetAllTeachersAsync(Guid? instituteId);
    }
}
