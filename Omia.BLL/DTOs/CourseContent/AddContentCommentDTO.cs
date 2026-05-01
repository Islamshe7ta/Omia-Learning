using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CourseContent
{
    public class AddContentCommentDTO
    {
        public Guid CourseContentId { get; set; }
        public string Message { get; set; }
    }
}
