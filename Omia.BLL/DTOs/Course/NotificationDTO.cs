using System;

namespace Omia.BLL.DTOs.Course
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public Guid ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
