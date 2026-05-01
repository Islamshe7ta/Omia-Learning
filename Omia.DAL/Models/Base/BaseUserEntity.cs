using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Omia.DAL.Models.Base
{
    public abstract class BaseUserEntity : BaseEntity
    {
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Display(Name = "كلمة المرور الأولية")]
        public string? InitPassword { get; set; }

        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Url]
        [Display(Name = "صورة شخصية")]
        public string? ProfileImageUrl { get; set; }
        
        [Required]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "حالة الحساب")]
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        [Display(Name = "متوقف مؤقتا حتى")]
        public DateTime? PausedUntil { get; set; }

        // Navigation Properties
        public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
    }
}
