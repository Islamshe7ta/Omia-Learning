using Omia.BLL.DTOs;
using Omia.BLL.DTOs.LiveSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ILiveSessionService
    {
        Task<CreateLiveSessionResponseDTO> CreateLiveSessionAsync(CreateLiveSessionDTO dto, Guid userId);
        Task<IEnumerable<GetLiveSessionResponseDTO>> GetLiveSessionsByCourseAsync(Guid courseId, Guid userId);
        Task<BaseResponseDTO> UpdateLiveSessionAsync(Guid liveSessionId, UpdateLiveSessionDTO dto, Guid userId);
        Task<BaseResponseDTO> DeleteLiveSessionAsync(Guid liveSessionId, Guid userId);
    }
}
