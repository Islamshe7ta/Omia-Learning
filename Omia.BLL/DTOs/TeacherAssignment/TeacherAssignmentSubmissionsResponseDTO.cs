using System.Collections.Generic;

namespace Omia.BLL.DTOs.TeacherAssignment
{
    public class TeacherAssignmentSubmissionsResponseDTO : BaseResponseDTO
    {
        public IEnumerable<AssignmentSubmissionTeacherDTO> Submissions { get; set; } = new List<AssignmentSubmissionTeacherDTO>();
    }
}
