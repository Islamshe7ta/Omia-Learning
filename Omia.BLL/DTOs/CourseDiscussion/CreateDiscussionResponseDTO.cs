using System;

namespace Omia.BLL.DTOs.CourseDiscussion
{
    public class CreateDiscussionResponseDTO : BaseResponseDTO
    {
        public Guid DiscussionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
