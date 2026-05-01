using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omia.BLL.DTOs;

namespace Omia.BLL.DTOs.CourseContent
{
    public class CreateCourseContentResponseDTO : BaseResponseDTO
    {
        public Guid ContentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
