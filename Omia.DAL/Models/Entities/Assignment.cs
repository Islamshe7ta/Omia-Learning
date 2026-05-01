using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الواجبات")]
    public class Assignment : BaseEntity
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

        [Url]
        [Display(Name = "ملف الواجب")]
        public string? AttachmentFile { get; set; }

        [Display(Name = "تاريخ التسليم")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "الدرجة القصوى")]
        public float? MaxGrade { get; set; }

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public virtual BaseUserEntity Creator { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    }
}
