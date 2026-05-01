using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.StudentAssignment
{
    public class MySubmissionDTO
    {
        public Guid Id { get; set; }
        public string? SubmittedFile { get; set; }
        public DateTime SubmittedAt { get; set; } 
        public float? Grade { get; set; }
        public string? CorrectedFile { get; set; }
        public string? TeacherComment { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
