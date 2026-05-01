namespace Omia.BLL.DTOs.CourseContent
{
    public class CourseCommentDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public SenderUserProfileDTO SenderUserProfile { get; set; } = null!;
    }
}