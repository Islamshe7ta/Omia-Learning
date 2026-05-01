using Microsoft.AspNetCore.Http;

namespace Omia.BLL.DTOs.Assignment
{
    public class CreateAssignmentDTO
    {

        public Guid CourseId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? AttachmentFile { get; set; }
        public IFormFile? File { get; set; }
        public DateTime? DueDate { get; set; }
        public int? MaxGrade { get; set; }
        public int? OrderNumber { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
