using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string UserType { get; set; }
        public StudentProfileDTO StudentUserProfile { get; set; }
        public TeacherProfileDTO TeacherUserProfile { get; set; }
        public AssistantProfileDTO AssistantUserProfile { get; set; }
        public InstituteProfileDTO InstituteUserProfile { get; set; }
        public ParentUserProfileDTO ParentUserProfile { get; set; }
    }
 
}
