using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Assignment
{
    public class UpdateAssignmentDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? AttachmentFile { get; set; }
        public IFormFile? File { get; set; }
        public DateTime? DueDate { get; set; }
        public int? MaxGrade { get; set; }
        public Guid? CategoryId { get; set; }
        public int? OrderNumber { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
