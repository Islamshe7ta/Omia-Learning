using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "تعليقات محتوى الكورس")]
    public class CourseContentComment : BaseEntity
    {
        [Required]
        public Guid CourseContentId { get; set; }
        [ForeignKey(nameof(CourseContentId))]
        public virtual CourseContent CourseContent { get; set; } = null!;

        [Required]
        [Display(Name = "الرسالة")]
        public string Message { get; set; } = string.Empty;

        [Required]
        public Guid SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public virtual BaseUserEntity Sender { get; set; } = null!;
    }
}
