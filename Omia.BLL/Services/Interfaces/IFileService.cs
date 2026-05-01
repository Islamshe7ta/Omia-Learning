using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Omia.BLL.DTOs.Profile;

namespace Omia.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<(bool IsSuccess, string Message, string? FileUrl)> UploadProfileImageAsync(ProfileImageUploadRequest request, string? oldImageUrl = null);
        Task<(bool IsSuccess, string Message, string? FileUrl)> UploadFileAsync(IFormFile file, string subPath, string? fileName = null);
        void DeleteFile(string fileUrl);
    }
}
