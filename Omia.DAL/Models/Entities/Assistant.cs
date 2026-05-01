using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الأستاذ المساعد")]
    public class Assistant : BaseUserEntity
    {
        [Display(Name = "التخصص")]
        public string? Specialization { get; set; }

        [Display(Name = "التحصيل الدراسي")]
        public string? EducationDegree { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        // Navigation Properties
        [Required]
        public Guid TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; } = null!;

        public virtual ICollection<AssistantCourse> AssistantCourses { get; set; } = new List<AssistantCourse>();
    }
}
