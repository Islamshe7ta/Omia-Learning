using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "Tokens")]
    public class UserToken : BaseEntity
    {
        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "ملاحظة")]
        public string? Note { get; set; }

        [Required]
        [Display(Name = "القيمة")]
        public string Value { get; set; } = string.Empty;

        [Required]
        [Display(Name = "تاريخ الانتهاء")]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public Guid UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public virtual BaseUserEntity User { get; set; } = null!;
    }
}
