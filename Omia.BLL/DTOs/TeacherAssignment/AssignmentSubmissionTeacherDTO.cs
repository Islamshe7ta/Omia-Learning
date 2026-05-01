using System;
using Omia.BLL.DTOs.Student;

namespace Omia.BLL.DTOs.TeacherAssignment
{
    public class AssignmentSubmissionTeacherDTO
    {
        public Guid Id { get; set; }
        public StudentBriefDTO Student { get; set; } = null!;
        public string? SubmittedFile { get; set; }
        public DateTime SubmittedAt { get; set; }
        public float? Grade { get; set; }
        public string? CorrectedFile { get; set; }
        public string? TeacherComment { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
