using System;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizAnswerDetailsDTO
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string TextAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public float MarksObtained { get; set; }
    }
}
