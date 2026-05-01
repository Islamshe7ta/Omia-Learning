using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "أسئلة الكويز")]
    public class QuizQuestion : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; } = null!;

        [Required]
        [Display(Name = "نص السؤال")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "نوع السؤال")]
        public QuestionType QuestionType { get; set; }

        [Display(Name = "الإجابات")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Answers { get; set; }

        [Required]
        [Display(Name = "الإجابة الصحيحة")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Display(Name = "معلومات إضافية")]
        [Column(TypeName = "nvarchar(max)")]
        public string? AdditionalInformation { get; set; }

        [Display(Name = "الدرجة")]
        public float? Marks { get; set; }

        [Required]
        [Display(Name = "ترتيب")]
        public int OrderNumber { get; set; }

        // Navigation Properties
        public virtual ICollection<QuizAnswer> QuizAnswers { get; set; } = new List<QuizAnswer>();
    }
}
