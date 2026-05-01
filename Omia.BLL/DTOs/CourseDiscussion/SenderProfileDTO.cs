using System;

namespace Omia.BLL.DTOs.CourseDiscussion
{
    public class SenderProfileDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string Subtitle { get; set; } = string.Empty;
    }
}
