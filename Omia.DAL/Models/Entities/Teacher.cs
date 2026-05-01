using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "الأستاذ")]
    public class Teacher : BaseUserEntity
    {
        [Url]
        [Display(Name = "اللوكو")]
        public string? LogoUrl { get; set; }

        [Display(Name = "التخصص")]
        public string? Specialization { get; set; }

        [Display(Name = "التحصيل الدراسي")]
        public string? EducationDegree { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "عدد المراحل الدراسية المتاحة له")]
        public int? AvailableStagesCount { get; set; }

        // Navigation Properties
        public virtual ICollection<Assistant> Assistants { get; set; } = new List<Assistant>();
        public virtual ICollection<Student> RegisteredStudents { get; set; } = new List<Student>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<CourseContent> CourseContents { get; set; } = new List<CourseContent>();
        public virtual ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        
        public Guid? InstituteId { get; set; }
        [ForeignKey(nameof(InstituteId))]
        public virtual Institute? Institute { get; set; }
    }
}
