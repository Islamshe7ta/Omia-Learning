using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "مساعدة الأستاذ في الكورس")]
    public class AssistantCourse : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [Required]
        public Guid AssistantId { get; set; }
        [ForeignKey(nameof(AssistantId))]
        public virtual Assistant Assistant { get; set; } = null!;

        [Display(Name = "الصلاحيات")]
        public AssistantPermissions Permissions { get; set; } = AssistantPermissions.None;
    }
}
