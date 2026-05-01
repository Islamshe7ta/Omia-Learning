using System.Collections.Generic;
using Omia.BLL.DTOs.QuizAttempt;

namespace Omia.BLL.DTOs.Quiz
{
    public class EndQuizRequestDTO
    {
        public Guid AttemptId { get; set; }
        public List<QuizAnswerDTO> Answers { get; set; } = new List<QuizAnswerDTO>();
    }
}
