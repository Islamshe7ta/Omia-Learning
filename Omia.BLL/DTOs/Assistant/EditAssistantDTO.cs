using System.ComponentModel.DataAnnotations;

namespace Omia.BLL.DTOs.Assistant
{
    public class EditAssistantDTO
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string Username { get; set; } = string.Empty;

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string? PhoneNumber { get; set; }

        public string? Specialization { get; set; }

        public string? EducationDegree { get; set; }

        public string? Address { get; set; }
    }
}
