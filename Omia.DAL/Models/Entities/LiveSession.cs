using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "المحاضرات المباشرة")]
    public class LiveSession : BaseEntity
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

        [Required]
        [Display(Name = "وقت البدء")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "وقت الانتهاء")]
        public DateTime EndTime { get; set; }

        [Url]
        [Display(Name = "رابط الاجتماع")]
        public string? MeetingLink { get; set; }

        [Url]
        [Display(Name = "رابط الفيديو المسجل")]
        public string? RecordedVideoUrl { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public virtual BaseUserEntity Creator { get; set; } = null!;
    }
}
