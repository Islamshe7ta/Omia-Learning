using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Assignment;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.DTOs.TeacherAssignment;

namespace Omia.BLL.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentResponseDTO> CreateAssignmentAsync(CreateAssignmentDTO createAssignmentDTO, Guid userId);
        Task<BaseResponseDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentDTO updateAssignmentDTO, Guid userId);
        Task<BaseResponseDTO> DeleteAssignmentAsync(Guid assignmentId, Guid userId);
        Task<IEnumerable<AssignmentsCourseResponse>> GetAssignmentsByCourseIdAsync(Guid courseId, Guid userId);
        Task<AssignmentSubmissionDTO> SubmitAssignmentAsync(SubmitAssignmentDTO requestDTO, Guid studentId);
        Task<IEnumerable<MySubmissionDTO>> GetMySubmissionsAsync(Guid assignmentId, Guid studentId);
        Task<CorrectAssignmentResponseDTO> CorrectSubmissionAsync(Guid submissionId, CorrectAssignmentSubmissionDTO dto, Guid teacherId);
        Task<TeacherAssignmentSubmissionsResponseDTO> GetAllSubmissionsAsync(Guid assignmentId, Guid teacherId);
    }
}
