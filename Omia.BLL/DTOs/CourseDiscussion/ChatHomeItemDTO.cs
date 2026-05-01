using System;

namespace Omia.BLL.DTOs.CourseDiscussion
{
    public class ChatHomeItemDTO
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? CourseImage { get; set; }
        public string? LastMessage { get; set; }
        public SenderProfileDTO? LastMessageSender { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int TotalMessages { get; set; }
    }
}
