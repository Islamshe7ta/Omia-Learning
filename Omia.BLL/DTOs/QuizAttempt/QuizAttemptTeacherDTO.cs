using Omia.BLL.DTOs.Student;
using Omia.DAL.Models.Enums;
using System;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class QuizAttemptTeacherDTO
    {
        public Guid Id { get; set; }
        public StudentBriefDTO Student { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Score { get; set; }
        public QuizAttemptStatus Status { get; set; }
    }
}
