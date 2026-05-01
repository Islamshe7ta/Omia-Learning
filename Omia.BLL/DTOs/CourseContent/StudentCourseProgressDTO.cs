namespace Omia.BLL.DTOs.CourseContent
{
    public class StudentCourseProgressDTO
    {
        public Guid Id { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}