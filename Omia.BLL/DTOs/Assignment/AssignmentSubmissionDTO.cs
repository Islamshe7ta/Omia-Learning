using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Assignment
{
    public class AssignmentSubmissionDTO :BaseResponseDTO
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public string? SubmittedFile { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
