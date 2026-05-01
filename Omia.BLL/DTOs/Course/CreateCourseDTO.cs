using Microsoft.AspNetCore.Http;

namespace Omia.BLL.DTOs.Course
{
    public class CreateCourseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? File { get; set; }
        public string? Subject { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid? TeacherId { get; set; }
        public float? Cost { get; set; }
    }
}
