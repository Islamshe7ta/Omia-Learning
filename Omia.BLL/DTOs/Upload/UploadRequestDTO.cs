using Microsoft.AspNetCore.Http;

namespace Omia.BLL.DTOs.Upload
{
    public class UploadRequestDTO
    {
        public IFormFile File { get; set; } = null!;
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
