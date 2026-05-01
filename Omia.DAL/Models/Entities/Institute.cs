using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "المعهد")]
    public class Institute : BaseUserEntity
    {
        [Required]
        [Display(Name = "اسم المعهد")]
        public string Name { get; set; } = string.Empty;

        [Url]
        [Display(Name = "اللوكو")]
        public string? LogoUrl { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "عدد المواد الدراسية المتاحة له")]
        public int? AvailableSubjectsCount { get; set; }

        // Navigation Properties
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Student> RegisteredStudents { get; set; } = new List<Student>();
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
