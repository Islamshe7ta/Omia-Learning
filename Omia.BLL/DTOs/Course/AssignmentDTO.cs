using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class AssignmentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AttachmentFile { get; set; }
        public DateTime? DueDate { get; set; }
        public float? MaxGrade { get; set; }
        public int OrderNumber { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
