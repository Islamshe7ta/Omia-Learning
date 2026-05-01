using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CentralAdmin
{
    public class CourseDetailDto
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int EnrolledStudents { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
