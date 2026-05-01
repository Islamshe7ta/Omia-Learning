namespace Omia.BLL.DTOs.Auth
{
    public class InstituteProfileDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } 
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public string? Address { get; set; }
    }
}