using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "تقسيمة الكورس")]
    public class CourseCategory : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        [Required]
        [Display(Name = "ترتيب")]
        public int OrderNumber { get; set; }

        // Navigation Properties
        public virtual ICollection<CourseContent> Contents { get; set; } = new List<CourseContent>();
        public virtual ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
