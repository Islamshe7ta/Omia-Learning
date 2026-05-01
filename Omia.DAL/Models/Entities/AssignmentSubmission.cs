using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "تسليم الواجبات")]
    public class AssignmentSubmission : BaseEntity
    {
        [Required]
        public Guid AssignmentId { get; set; }
        [ForeignKey(nameof(AssignmentId))]
        public virtual Assignment Assignment { get; set; } = null!;

        [Required]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [Url]
        [Display(Name = "الملف المرفوع")]
        public string? SubmittedFile { get; set; }

        [Display(Name = "الدرجة")]
        public float? Grade { get; set; }

        [Url]
        [Display(Name = "ملف التصحيح")]
        public string? CorrectedFile { get; set; }

        [Display(Name = "ملاحظات المدرس")]
        public string? TeacherComment { get; set; }
        
        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        public Guid? CorrectedBy { get; set; }
        [ForeignKey(nameof(CorrectedBy))]
        public virtual BaseUserEntity? Corrector { get; set; }
    }
}
