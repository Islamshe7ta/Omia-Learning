using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class CoursesResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public IEnumerable<StudentCourseDTO> Courses { get; set; }
    }
}
