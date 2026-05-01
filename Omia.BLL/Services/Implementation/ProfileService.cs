using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Profile;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProfileResponseDTO> GetProfileAsync(Guid userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null || user.IsDeleted)
            {
                return new ProfileResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            var profileDto = _mapper.Map<ProfileDTO>(user);

            return new ProfileResponseDTO
            {
                IsSuccess = true,
                Message = "Profile retrieved successfully",
                Profile = profileDto
            };
        }

        public async Task<BaseResponseDTO> UpdateProfileAsync(Guid userId, UpdateProfileDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.Username)
            {
                if (await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, userId))
                    return new BaseResponseDTO { IsSuccess = false, Message = "Username already exists" };
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                if (await _unitOfWork.Users.IsEmailExistsAsync(request.Email, userId))
                    return new BaseResponseDTO { IsSuccess = false, Message = "Email already exists" };
            }

            _mapper.Map(request, user);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Profile updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdateFullnameAsync(Guid userId, UpdateFullnameDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            if (string.IsNullOrWhiteSpace(request.Fullname))
                return new BaseResponseDTO { IsSuccess = false, Message = "Fullname cannot be empty" };

            _mapper.Map(request, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Fullname updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdateUsernameAsync(Guid userId, UpdateUsernameDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            if (string.IsNullOrWhiteSpace(request.Username))
                return new BaseResponseDTO { IsSuccess = false, Message = "Username cannot be empty" };

            if (request.Username != user.Username && await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, userId))
                return new BaseResponseDTO { IsSuccess = false, Message = "Username already exists" };

            _mapper.Map(request, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Username updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdateEmailAsync(Guid userId, UpdateEmailDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            if (string.IsNullOrWhiteSpace(request.Email))
                return new BaseResponseDTO { IsSuccess = false, Message = "Email cannot be empty" };

            if (request.Email != user.Email && await _unitOfWork.Users.IsEmailExistsAsync(request.Email, userId))
                return new BaseResponseDTO { IsSuccess = false, Message = "Email already exists" };

            _mapper.Map(request, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Email updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdatePhoneNumberAsync(Guid userId, UpdatePhoneNumberDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            _mapper.Map(request, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Phone number updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdateLocationAsync(Guid userId, UpdateLocationDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            _mapper.Map(request, user, typeof(UpdateLocationDTO), user.GetType());
            
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Location updated successfully" };
        }

        public async Task<BaseResponseDTO> ChangePasswordAsync(Guid userId, ChangePasswordDTO request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new BaseResponseDTO { IsSuccess = false, Message = "User not found" };

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "Current password is incorrect" };
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Password changed successfully" };
        }

        public async Task<UpdateProfileImageResponseDTO> UpdateProfileImageAsync(Guid userId, ProfileImageUploadRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return new UpdateProfileImageResponseDTO { IsSuccess = false, Message = "User not found" };

            var uploadResult = await _fileService.UploadProfileImageAsync(request, user.ProfileImageUrl);

            if (!uploadResult.IsSuccess)
            {
                return new UpdateProfileImageResponseDTO { IsSuccess = false, Message = uploadResult.Message };
            }
            
            user.ProfileImageUrl = uploadResult.FileUrl;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new UpdateProfileImageResponseDTO 
            { 
                IsSuccess = true, 
                Message = "Profile image updated successfully",
                ProfileImageUrl = uploadResult.FileUrl
            };
        }
    }
}
