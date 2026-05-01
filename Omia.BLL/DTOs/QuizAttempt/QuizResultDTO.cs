using System;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizResultDTO
    {
        public Guid AttemptId { get; set; }
        public float Score { get; set; }
        public float TotalMarks { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
