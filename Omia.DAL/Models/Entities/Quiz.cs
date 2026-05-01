using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الكويزات")]
    public class Quiz : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual CourseCategory? Category { get; set; }

        [Required]
        [Display(Name = "ترتيب")]
        public int OrderNumber { get; set; }

        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "المدة بالدقائق")]
        public int? DurationMinutes { get; set; }

        [Display(Name = "الدرجة الكلية")]
        public float? TotalMarks { get; set; }

        [Display(Name = "تاريخ البدء")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public virtual BaseUserEntity Creator { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
