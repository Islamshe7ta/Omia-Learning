using System;

namespace Omia.BLL.DTOs.CourseContent
{
    public class VideoSummaryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid CategoryId { get; set; }
    }
}
