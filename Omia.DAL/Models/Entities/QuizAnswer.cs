using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "إجابات الأسئلة")]
    public class QuizAnswer : BaseEntity
    {
        [Required]
        public Guid AttemptId { get; set; }
        [ForeignKey(nameof(AttemptId))]
        public virtual QuizAttempt Attempt { get; set; } = null!;

        [Required]
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual QuizQuestion Question { get; set; } = null!;

        [Display(Name = "الإجابة النصية")]
        public string? TextAnswer { get; set; }

        [Display(Name = "صحيح")]
        public bool? IsCorrect { get; set; }

        [Display(Name = "الدرجة المحصلة")]
        public float? MarksObtained { get; set; }
    }
}
