using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Assignment;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.DTOs.TeacherAssignment;
using Omia.BLL.Helpers;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Services.Implementation
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<AssignmentResponseDTO> CreateAssignmentAsync(CreateAssignmentDTO createAssignmentDTO, Guid userId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(createAssignmentDTO.CourseId);
            if (course == null)
            {
                return new AssignmentResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                           _unitOfWork,
                           userId,
                           createAssignmentDTO.CourseId,
                           AssistantPermissions.AssignmentReviewer
                       );
            if (!hasPermission)
            {
                return new AssignmentResponseDTO { IsSuccess = false, Message = "You are not allowed to create assignments" };
            }

            if (createAssignmentDTO.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CourseCategories.GetByIdAsync(createAssignmentDTO.CategoryId.Value);
                if (category == null)
                {
                    return new AssignmentResponseDTO { IsSuccess = false, Message = "Category not found" };
                }
            }

            var assignment = _mapper.Map<Assignment>(createAssignmentDTO);
            assignment.CreatedBy = userId;
            assignment.CreatedAt = DateTime.UtcNow;

            if (!createAssignmentDTO.OrderNumber.HasValue)
            {
                var maxOrder = await _unitOfWork.Assignments.MaxAsync(a => a.CategoryId == createAssignmentDTO.CategoryId && a.CourseId == createAssignmentDTO.CourseId, a => a.OrderNumber);
                assignment.OrderNumber = maxOrder + 1;
            }

            if (createAssignmentDTO.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(createAssignmentDTO.File, "assignments");
                if (uploadResult.IsSuccess)
                {
                    assignment.AttachmentFile = uploadResult.FileUrl;
                }
            }

            await _unitOfWork.Assignments.AddAsync(assignment);
            await _unitOfWork.CommitAsync();

            return new AssignmentResponseDTO
            {
                IsSuccess = true,
                Message = "Assignment created successfully",
                AssignmentId = assignment.Id,
                CreatedAt = assignment.CreatedAt
            };
        }

        public async Task<BaseResponseDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentDTO updateAssignmentDTO, Guid userId)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            if (assignment == null)
            {
                return new AssignmentResponseDTO { IsSuccess = false, Message = "Assignment not found" };
            }

            var course = await _unitOfWork.Courses.GetByIdAsync(assignment.CourseId);
            if (course == null)
            {
                return new AssignmentResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                           _unitOfWork,
                           userId,
                           assignment.CourseId,
                           AssistantPermissions.AssignmentReviewer
                       );
            if (!hasPermission)
            {
                return new AssignmentResponseDTO { IsSuccess = false, Message = "You are not allowed to update assignments" };
            }

            if (updateAssignmentDTO.CategoryId.HasValue)
            {
                var category = await _unitOfWork.CourseCategories.GetByIdAsync(updateAssignmentDTO.CategoryId.Value);
                if (category == null || category.CourseId != assignment.CourseId)
                {
                    return new AssignmentResponseDTO { IsSuccess = false, Message = "Category not found or does not belong to this course" };
                }
            }


            var originalOrder = assignment.OrderNumber;
            _mapper.Map(updateAssignmentDTO, assignment);

            if (!updateAssignmentDTO.OrderNumber.HasValue)
            {
                assignment.OrderNumber = originalOrder;
            }

            if (updateAssignmentDTO.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(updateAssignmentDTO.File, "assignments");
                if (uploadResult.IsSuccess)
                {
                    assignment.AttachmentFile = uploadResult.FileUrl;
                }
            }

            assignment.LastUpdatedAt = DateTime.UtcNow;
            _unitOfWork.Assignments.Update(assignment);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO
            {
                IsSuccess = true,
                Message = "Assignment updated successfully",

            };

        }

        public async Task<BaseResponseDTO> DeleteAssignmentAsync(Guid assignmentId, Guid userId)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            if (assignment == null) return new BaseResponseDTO { IsSuccess = false, Message = "Assignment not found" };

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, userId, assignment.CourseId, AssistantPermissions.AssignmentReviewer);
            if (!hasPermission) return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to delete assignments" };

            _unitOfWork.Assignments.Delete(assignment);
            await _unitOfWork.CommitAsync();
            return new BaseResponseDTO { IsSuccess = true, Message = "Assignment deleted successfully" };

        }

        public async Task<IEnumerable<AssignmentsCourseResponse>> GetAssignmentsByCourseIdAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);

            if (!hasAccess)
            {
                return Enumerable.Empty<AssignmentsCourseResponse>();
            }

            var assignments = await _unitOfWork.Assignments.GetAssignmentsByCourseId(courseId, userId);
            if (assignments == null || !assignments.Any())
            {
                return Enumerable.Empty<AssignmentsCourseResponse>();
            }

            var mappedAssignments = _mapper.Map<IEnumerable<AssignmentsCourseResponse>>(assignments);
            return mappedAssignments;

        }

        public async Task<AssignmentSubmissionDTO> SubmitAssignmentAsync(SubmitAssignmentDTO requestDTO, Guid studentId)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(requestDTO.AssignmentId);
            if (assignment == null)
            {
                return new AssignmentSubmissionDTO { IsSuccess = false, Message = "Assignment not found" };
            }

            var studentIsEnrolled = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, studentId, assignment.CourseId);
            if (!studentIsEnrolled)
            {
                return new AssignmentSubmissionDTO { IsSuccess = false, Message = "Student not enrolled in this course" };
            }

            if (assignment.DueDate.HasValue && assignment.DueDate.Value < DateTime.UtcNow)
            {
                return new AssignmentSubmissionDTO { IsSuccess = false, Message = "Assignment submission deadline passed" };
            }

            var submission = _mapper.Map<AssignmentSubmission>(requestDTO);
            submission.StudentId = studentId;

            if (requestDTO.File != null)
            {
                var uploadResult = await _fileService.UploadFileAsync(requestDTO.File, "submissions");
                if (uploadResult.IsSuccess)
                {
                    submission.SubmittedFile = uploadResult.FileUrl;
                }
            }

            await _unitOfWork.AssignmentSubmissions.AddAsync(submission);
            await _unitOfWork.CommitAsync();

            return new AssignmentSubmissionDTO
            {
                IsSuccess = true,
                Message = "Assignment submitted successfully",
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                SubmittedFile = submission.SubmittedFile,
                AdditionalInformation = submission.AdditionalInformation,
                SubmittedAt = submission.CreatedAt 
            };
        }

        public async Task<IEnumerable<MySubmissionDTO>> GetMySubmissionsAsync(Guid assignmentId, Guid studentId)
        {
           
            var submissions =await _unitOfWork.AssignmentSubmissions.GetMySubmissionsAsync(assignmentId, studentId);
            if (submissions == null || !submissions.Any())
            {
                return Enumerable.Empty<MySubmissionDTO>();
            }
            var mappedSubmissions = _mapper.Map<IEnumerable<MySubmissionDTO>>(submissions);
            return mappedSubmissions;
           
        }

        public async Task<CorrectAssignmentResponseDTO> CorrectSubmissionAsync(Guid submissionId, CorrectAssignmentSubmissionDTO dto, Guid teacherId)
        {
            var submission = await _unitOfWork.AssignmentSubmissions.GetByIdAsync(submissionId);
            if (submission == null)
            {
                return new CorrectAssignmentResponseDTO { IsSuccess = false, Message = "Submission not found" };
            }

            var assignment = await _unitOfWork.Assignments.GetByIdAsync(submission.AssignmentId);
            if (assignment == null)
            {
                return new CorrectAssignmentResponseDTO { IsSuccess = false, Message = "Assignment not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork, teacherId, assignment.CourseId, AssistantPermissions.AssignmentReviewer);
            
            if (!hasPermission)
            {
                return new CorrectAssignmentResponseDTO { IsSuccess = false, Message = "You are not allowed to correct assignments" };
            }

            _mapper.Map(dto, submission);
            submission.CorrectedBy = teacherId;
            submission.LastUpdatedAt = DateTime.UtcNow;

            _unitOfWork.AssignmentSubmissions.Update(submission);
            await _unitOfWork.CommitAsync();

            return new CorrectAssignmentResponseDTO
            {
                IsSuccess = true,
                Message = "Assignment corrected successfully",
                Id = submission.Id,
                SubmittedFile = submission.SubmittedFile,
                SubmittedAt = submission.CreatedAt,
                Grade = submission.Grade,
                CorrectedFile = submission.CorrectedFile,
                TeacherComment = submission.TeacherComment,
                AdditionalInformation = submission.AdditionalInformation
            };
        }

        public async Task<TeacherAssignmentSubmissionsResponseDTO> GetAllSubmissionsAsync(Guid assignmentId, Guid teacherId)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(assignmentId);
            if (assignment == null)
            {
                return new TeacherAssignmentSubmissionsResponseDTO { IsSuccess = false, Message = "Assignment not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork, teacherId, assignment.CourseId, AssistantPermissions.AssignmentReviewer);
            
            if (!hasPermission)
            {
                return new TeacherAssignmentSubmissionsResponseDTO { IsSuccess = false, Message = "You are not allowed to view submissions" };
            }

            var submissions = await _unitOfWork.AssignmentSubmissions.GetAllSubmissionsByAssignmentAsync(assignmentId);
            var mappedSubmissions = _mapper.Map<IEnumerable<AssignmentSubmissionTeacherDTO>>(submissions);

            return new TeacherAssignmentSubmissionsResponseDTO
            {
                IsSuccess = true,
                Message = "Submissions retrieved successfully",
                Submissions = mappedSubmissions
            };
        }
    }
}
