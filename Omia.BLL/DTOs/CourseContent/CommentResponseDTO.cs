using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CourseContent
{
    public class CommentResponseDTO : BaseResponseDTO
    {
        public Guid CommentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
