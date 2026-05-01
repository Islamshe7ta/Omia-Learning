using System;
using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.AssistantCourse
{
    public class UpdateAssistantRolesDTO
    {
        [Required(ErrorMessage = "Assistant ID is required")]
        public Guid AssistantId { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public Guid CourseId { get; set; }

        public int Permissions { get; set; }
    }
}
