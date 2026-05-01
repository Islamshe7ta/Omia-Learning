using Omia.DAL.Models.Enums;

namespace Omia.BLL.DTOs.QuizQuestion
{
    public class CreateQuizQuestionDTO
    {

        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Answers { get; set; }
        public string CorrectAnswer { get; set; }
        public float Marks { get; set; }
        public int OrderNumber { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}