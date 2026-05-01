using System;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizAttemptStartDTO
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime EndTimeAllowed { get; set; }
    }
}
