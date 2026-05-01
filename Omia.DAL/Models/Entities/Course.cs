using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الكورس")]
    public class Course : BaseEntity
    {
        [Required]
        [Display(Name = "اسم الكورس")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "السعر")]
        public float? Cost { get; set; }

        [Required]
        [Display(Name = "الوصف")]
        public string Description { get; set; } = string.Empty;

        [Url]
        [Display(Name = "صورة الكورس")]
        public string? Image { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "مادة الكورس")]
        public string? Subject { get; set; }

        // Foreign Keys
        public Guid TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; } = null!;

        public Guid? InstituteId { get; set; }
        [ForeignKey(nameof(InstituteId))]
        public virtual Institute? Institute { get; set; }

        // Navigation Properties
        public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public virtual ICollection<AssistantCourse> AssistantCourses { get; set; } = new List<AssistantCourse>();
        public virtual ICollection<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
        public virtual ICollection<CourseContent> Contents { get; set; } = new List<CourseContent>();
        public virtual ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<CourseDiscussion> Discussions { get; set; } = new List<CourseDiscussion>();
        public virtual ICollection<CourseProgress> Progresses { get; set; } = new List<CourseProgress>();
    }
}
