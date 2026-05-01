using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "محتوى الكورس")]
    public class CourseContent : BaseEntity
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual CourseCategory? Category { get; set; }

        [Required]
        [Display(Name = "العنوان")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "نوع المحتوى")]
        public ContentType ContentType { get; set; }

        [Required]
        [Url]
        [Display(Name = "الرابط")]
        public string Url { get; set; } = string.Empty;

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        [Required]
        [Display(Name = "ترتيب")]
        public int OrderNumber { get; set; }

        [Display(Name = "مجاني")]
        public bool IsFree { get; set; } = false;

        [Required]
        public Guid UploadedBy { get; set; }
        [ForeignKey(nameof(UploadedBy))]
        public virtual BaseUserEntity Uploader { get; set; } = null!;
        
        public virtual ICollection<CourseContentComment> Comments { get; set; } = new List<CourseContentComment>();
    }
}
