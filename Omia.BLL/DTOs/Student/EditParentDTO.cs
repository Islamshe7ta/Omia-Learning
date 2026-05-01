using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.Student
{
    public class EditParentDTO
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? Password { get; set; } = string.Empty;
    }
}
