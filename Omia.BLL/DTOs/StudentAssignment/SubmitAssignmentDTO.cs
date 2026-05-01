using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.StudentAssignment
{
    public class SubmitAssignmentDTO
    {
        public Guid AssignmentId { get; set; }
        public string? SubmittedFile { get; set; }
        public IFormFile? File { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
