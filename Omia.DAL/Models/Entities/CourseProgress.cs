using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "تقدم الطالب")]
    public class CourseProgress : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        [Required]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [Required]
        public Guid ContentId { get; set; }
        [ForeignKey(nameof(ContentId))]
        public virtual CourseContent Content { get; set; } = null!;

        [Display(Name = "مكتمل")]
        public bool IsCompleted { get; set; } = false;

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }
    }
}
