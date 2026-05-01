using System;

namespace Omia.BLL.DTOs.TeacherAssignment
{
    public class CorrectAssignmentSubmissionDTO
    {
        public float Grade { get; set; }
        public string? CorrectedFile { get; set; }
        public string? TeacherComment { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
