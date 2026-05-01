using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "المناقشات")]
    public class CourseDiscussion : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [Required]
        public Guid SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual BaseUserEntity Sender { get; set; } = null!;

        [Display(Name = "المستلم")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Receiver { get; set; }

        [Required]
        [Display(Name = "الرسالة")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Display(Name = "نوع الرسالة")]
        public MessageType MessageType { get; set; }

        [Required]
        [Display(Name = "حالة الرسالة")]
        public MessageStatus Status { get; set; } = MessageStatus.Sent;

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }
    }
}
