using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CourseContent
{
    public class ContentDetailsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ContentType { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
        public int OrderNumber { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public StudentCourseProgressDTO? StudentCourseProgress { get; set; }
        public List<CourseCommentDTO> Comments { get; set; } = new List<CourseCommentDTO>();
    }
}
