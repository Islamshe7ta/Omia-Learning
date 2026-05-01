using System;
using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.AssistantCourse
{
    public class RemoveAssistantFromCourseDTO
    {
        [Required(ErrorMessage = "Assistant ID is required")]
        public Guid AssistantId { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public Guid CourseId { get; set; }
    }
}
