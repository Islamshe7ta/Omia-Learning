namespace Omia.BLL.DTOs.CourseContent
{
    public class SenderUserProfileDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string Subtitle { get; set; } = string.Empty; // Student, Teacher, Assistant
    }
}