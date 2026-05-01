using System;
using System.Collections.Generic;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizAttemptDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Score { get; set; }
        public List<QuizAnswerDetailsDTO> Answers { get; set; } = new();
    }
}
