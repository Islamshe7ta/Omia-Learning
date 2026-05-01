using System;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizAnswerDTO
    {
        public Guid QuestionId { get; set; }
        public string TextAnswer { get; set; } = string.Empty;
    }
}
