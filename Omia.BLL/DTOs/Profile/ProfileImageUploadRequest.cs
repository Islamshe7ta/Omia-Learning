using System.IO;

namespace Omia.BLL.DTOs.Profile
{
    public class ProfileImageUploadRequest
    {
        public Stream FileStream { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Length { get; set; }
    }
}
