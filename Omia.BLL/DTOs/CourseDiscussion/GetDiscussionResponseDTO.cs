using System.Collections.Generic;

namespace Omia.BLL.DTOs.CourseDiscussion
{
    public class GetDiscussionResponseDTO
    {
        public SenderProfileDTO SenderUserProfile { get; set; } = null!;
        public string Receiver { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
    }
}
