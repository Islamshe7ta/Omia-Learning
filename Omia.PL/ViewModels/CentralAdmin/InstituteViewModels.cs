using System.ComponentModel.DataAnnotations;

namespace Omia.PL.ViewModels.CentralAdmin
{
    public class InstituteListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int StudentCount { get; set; }
        public int ActiveCourseCount { get; set; }
        public int? AvailableSubjectsCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class InstituteFormViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "اسم المعهد مطلوب")]
        [Display(Name = "اسم المعهد")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [Display(Name = "اسم المستخدم")]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "كلمة المرور")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string? Password { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "عدد المواد الدراسية المتاحة")]
        [Range(0, 10000)]
        public int? AvailableSubjectsCount { get; set; }

        public bool IsEdit => Id.HasValue && Id != Guid.Empty;
    }
}
