namespace Omia.BLL.DTOs.Auth
{
    public class StudentProfileDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } 
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? EducationStage { get; set; }
        public string? Governorate { get; set; }

    }
}
