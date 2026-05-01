namespace Omia.BLL.DTOs.Quiz
{
    using Omia.BLL.DTOs.QuizQuestion;

    public class UpdateQuizDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public float? TotalMarks { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? OrderNumber { get; set; }
        public string? AdditionalInformation { get; set; }

        public Guid? CategoryId { get; set; }

        public IEnumerable<CreateQuizQuestionDTO> Questions { get; set; } = new List<CreateQuizQuestionDTO>();
    }
}
