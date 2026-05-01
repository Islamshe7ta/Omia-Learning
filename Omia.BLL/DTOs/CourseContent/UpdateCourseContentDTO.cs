using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CourseContent
{
    public class UpdateCourseContentDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ContentType { get; set; }
        public string? Url { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        public string? AdditionalInformation { get; set; }
        public int? OrderNumber { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
