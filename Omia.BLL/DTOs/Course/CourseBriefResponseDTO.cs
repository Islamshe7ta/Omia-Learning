using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class CourseBriefResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public CourseBriefDTO Data { get; set; }
    }
}
