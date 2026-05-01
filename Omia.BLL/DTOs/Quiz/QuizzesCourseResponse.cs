using System;

namespace Omia.BLL.DTOs.Quiz
{
    public class QuizzesCourseResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public float? TotalMarks { get; set; }
        public DateTime? StartTime { get; set; } 
        public DateTime? EndTime { get; set; }   
        public Guid? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
