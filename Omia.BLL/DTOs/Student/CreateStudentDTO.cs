using System;
using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.Student
{
    public class CreateStudentDTO
    {
       
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string? EducationStage { get; set; }
        public string? Governorate { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public Guid CourseId { get; set; }

      
        public string? ParentFullName { get; set; } = string.Empty;

        public string? ParentEmail { get; set; }

        [Required(ErrorMessage = "Parent Username is required")]
        public string? ParentUsername { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid Parent Phone Number")]
        public string? ParentPhoneNumber { get; set; }

        public string? ParentPassword { get; set; } = string.Empty;
    }
}
