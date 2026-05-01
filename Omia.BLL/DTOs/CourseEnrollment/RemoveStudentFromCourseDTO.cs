using System;
using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.CourseEnrollment
{
    public class RemoveStudentFromCourseDTO
    {
        [Required(ErrorMessage = "Student ID is required")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public Guid CourseId { get; set; }
    }
}
