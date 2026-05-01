using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.CourseContent
{
    public class CreateCourseContentDTO
    {
        public Guid CourseId { get; set; }
        
        public Guid? CategoryId { get; set; }
       
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public int ContentType { get; set; }
        
        public string? Url { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        
        public string? AdditionalInformation { get; set; }
        
        public int? OrderNumber { get; set; }
    }
}
