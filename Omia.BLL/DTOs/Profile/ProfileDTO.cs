using System;

namespace Omia.BLL.DTOs.Profile
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; } 
        public string? ProfileImageUrl { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? EducationStage { get; set; }
        public string? Governorate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
