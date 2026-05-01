namespace Omia.BLL.DTOs.QuizQuestion
{
    public class QuizQuestionDetailsDTO
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty; 
        public string Answers { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public float Marks { get; set; }
        public int OrderNumber { get; set; }
    }
}
