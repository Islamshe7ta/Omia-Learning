using System;

namespace Omia.BLL.DTOs.Course
{
    public class CourseCreateResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Guid CourseId { get; set; }
    }
}
