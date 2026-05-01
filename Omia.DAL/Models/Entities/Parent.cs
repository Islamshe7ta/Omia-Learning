using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "ولي الامر")]
    public class Parent : BaseUserEntity
    {
        // Navigation Properties
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
