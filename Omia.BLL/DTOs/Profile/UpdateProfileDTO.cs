using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.Profile
{
    public class UpdateProfileDTO
    {
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; } 
    }
}
