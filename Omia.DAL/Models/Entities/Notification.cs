using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الإشعارات")]
    public class Notification : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual BaseUserEntity User { get; set; } = null!;

        [Required]
        [Display(Name = "نوع الإشعار")]
        public string Type { get; set; } = string.Empty;

        public Guid? ReferenceId { get; set; }

        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "تمت القراءة")]
        public bool IsRead { get; set; } = false;
    }
}
