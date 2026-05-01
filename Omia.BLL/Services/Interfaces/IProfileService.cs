using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Profile;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileResponseDTO> GetProfileAsync(Guid userId);
        Task<BaseResponseDTO> UpdateProfileAsync(Guid userId, UpdateProfileDTO request);
        Task<BaseResponseDTO> UpdateFullnameAsync(Guid userId, UpdateFullnameDTO request);
        Task<BaseResponseDTO> UpdateUsernameAsync(Guid userId, UpdateUsernameDTO request);
        Task<BaseResponseDTO> UpdateEmailAsync(Guid userId, UpdateEmailDTO request);
        Task<BaseResponseDTO> UpdatePhoneNumberAsync(Guid userId, UpdatePhoneNumberDTO request);
        Task<BaseResponseDTO> UpdateLocationAsync(Guid userId, UpdateLocationDTO request);
        Task<BaseResponseDTO> ChangePasswordAsync(Guid userId, ChangePasswordDTO request);
        Task<UpdateProfileImageResponseDTO> UpdateProfileImageAsync(Guid userId, ProfileImageUploadRequest request);
    }
}
