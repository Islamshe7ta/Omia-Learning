using Omia.BLL.DTOs;
using Omia.BLL.DTOs.AssistantCourse;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface IAssistantCourseService
    {
        Task<BaseResponseDTO> AssignAssistantToCourseAsync(AssignAssistantToCourseDTO request);
        Task<BaseResponseDTO> RemoveAssistantFromCourseAsync(Guid assistantId, Guid courseId);
        Task<BaseResponseDTO> UpdateAssistantRolesAsync(UpdateAssistantRolesDTO request);
    }
}
