using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "محاولات الكويز")]
    public class QuizAttempt : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; } = null!;

        [Required]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [Required]
        [Display(Name = "وقت البدء")]
        public DateTime StartTime { get; set; }

        [Display(Name = "وقت الانتهاء")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "الدرجة")]
        public float? Score { get; set; }

        [Required]
        [Display(Name = "الحالة")]
        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

        // Navigation Properties
        public virtual ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
    }
}
