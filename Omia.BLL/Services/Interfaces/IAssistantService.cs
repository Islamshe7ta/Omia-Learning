using Omia.BLL.DTOs.Assistant;
using System;
using System.Threading.Tasks;

using Omia.BLL.DTOs.Auth;
using System.Collections.Generic;
using Omia.BLL.DTOs;

namespace Omia.BLL.Services.Interfaces
{
    public interface IAssistantService
    {
        Task<AssistantCreatedResponseDTO> CreateAssistantAsync(CreateAssistantDTO request, Guid userId);
        Task<BaseResponseDTO> UpdateAssistantAsync(Guid assistantId, EditAssistantDTO request, Guid userId);
        Task<BaseResponseDTO> DeleteAssistantAsync(Guid assistantId, Guid userId);
        Task<AssistantProfileDTO?> GetAssistantByIdAsync(Guid assistantId, Guid userId);
        Task<IEnumerable<AssistantProfileDTO>?> GetAllAssistantsAsync(Guid userId, Guid? teacherId);
    }
}
