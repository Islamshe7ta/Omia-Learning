using System.Threading.Tasks;
using Omia.BLL.DTOs.QuizQuestion;

namespace Omia.BLL.DTOs.Quiz
{
    public class CreateQuizDTO
    {

        public Guid CourseId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public float? TotalMarks { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? OrderNumber { get; set; }
        public string? AdditionalInformation { get; set; }
        public List<CreateQuizQuestionDTO> Questions { get; set; } = new List<CreateQuizQuestionDTO>();
    }
}
