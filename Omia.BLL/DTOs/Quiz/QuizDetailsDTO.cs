using System.Collections.Generic;
using Omia.BLL.DTOs.QuizQuestion;

namespace Omia.BLL.DTOs.Quiz
{
    public class QuizDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public float TotalMarks { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<QuizQuestionDetailsDTO> Questions { get; set; } = new List<QuizQuestionDetailsDTO>();
    }
}
