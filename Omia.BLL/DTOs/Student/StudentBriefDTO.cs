using System;

namespace Omia.BLL.DTOs.Student
{
    public class StudentBriefDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? EducationStage { get; set; }
    }
}
