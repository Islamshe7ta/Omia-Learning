using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Omia.BLL.DTOs.Profile;
using Omia.BLL.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Omia.BLL.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
        private readonly string _baseUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public FileService()
        {
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<(bool IsSuccess, string Message, string? FileUrl)> UploadProfileImageAsync(ProfileImageUploadRequest request, string? oldImageUrl = null)
        {
            if (request.Length == 0)
                return (false, "File is empty.", null);

            if (request.Length > 5 * 1024 * 1024)
                return (false, "Image size exceeds allowed limit (5MB).", null);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(request.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return (false, "Invalid image format.", null);

            // Read stream into MemoryStream for ImageSharp validation and saving
            using var memoryStream = new MemoryStream();
            await request.FileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            try
            {
                using var image = await Image.LoadAsync(memoryStream);
                
                if (image.Width < 200 || image.Height < 200)
                {
                    return (false, "Image resolution must be at least 200x200 px.", null);
                }
            }
            catch (UnknownImageFormatException)
            {
                return (false, "Invalid image format.", null);
            }
            catch (Exception)
            {
                return (false, "Failed to process the image file.", null);
            }

            memoryStream.Position = 0;

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadDirectory, uniqueFileName);

            // Delete old file asynchronously to avoid blocking
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                DeleteFile(oldImageUrl);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await memoryStream.CopyToAsync(fileStream);
            }

            // Return relative URL for storage
            return (true, "Image uploaded successfully", $"/uploads/profiles/{uniqueFileName}");
        }

        public async Task<(bool IsSuccess, string Message, string? FileUrl)> UploadFileAsync(IFormFile file, string subPath, string? fileName = null)
        {
            if (file == null || file.Length == 0)
                return (false, "File is empty.", null);

            try
            {
                var uploadPath = Path.Combine(_baseUploadDirectory, subPath.TrimStart('/'));
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var extension = Path.GetExtension(file.FileName);
                var finalFileName = !string.IsNullOrEmpty(fileName) 
                    ? $"{fileName}{extension}" 
                    : $"{Guid.NewGuid()}{extension}";

                var filePath = Path.Combine(uploadPath, finalFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativeUrl = $"/uploads/{subPath.TrimStart('/')}/{finalFileName}".Replace("//", "/");
                return (true, "File uploaded successfully", relativeUrl);
            }
            catch (Exception ex)
            {
                return (false, $"Failed to upload file: {ex.Message}", null);
            }
        }

        public void DeleteFile(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return;

            var pathParts = fileUrl.TrimStart('/').Split('/');
            if (pathParts.Length < 2 || pathParts[0] != "uploads") return;

            var relativePath = Path.Combine(pathParts.Skip(1).ToArray());
            var filePath = Path.Combine(_baseUploadDirectory, relativePath);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    // Ignore deletion errors gracefully
                }
            }
        }
    }
}
