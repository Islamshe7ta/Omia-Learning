using System;
using System.Collections.Generic;

namespace Omia.BLL.DTOs.CourseDiscussion
{
    public class CreateDiscussionMessageDTO
    {
        public Guid CourseId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = "Text";
        public string Receiver { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
    }
}
