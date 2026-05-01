using Omia.BLL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class StudentCourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? Cost { get; set; }
        public string Description { get; set; }
        public string? Subject { get; set; }
        public string Image { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int StudentsCount { get; set; }
        public IEnumerable<AssistantProfileDTO> AssistantUserProfiles { get; set; }
        public TeacherProfileDTO TeacherUserProfile { get; set; }
        public InstituteProfileDTO InstituteUserProfile { get; set; }

    }
}
