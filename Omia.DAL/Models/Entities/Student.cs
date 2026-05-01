using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الطالب")]
    public class Student : BaseUserEntity
    {

        [Display(Name = "المرحلة")]
        public string? EducationStage { get; set; }

        [Display(Name = "المحافظة")]
        public string? Governorate { get; set; }

        // Relationships
        public Guid? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual Parent? Parent { get; set; }

        public Guid? TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher? Teacher { get; set; }

        public Guid? InstituteId { get; set; }
        [ForeignKey(nameof(InstituteId))]
        public virtual Institute? Institute { get; set; }

        public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public virtual ICollection<CourseProgress> CourseProgresses { get; set; } = new List<CourseProgress>();
    }
}
