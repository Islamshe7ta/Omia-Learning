using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Assignment;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.DTOs.TeacherAssignment;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class AssignmentApiController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentApiController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Authorize]
        [HttpGet("api/courses/{courseId}/assignments")]
        public async Task<IActionResult> GetAssignmentsByCourseId(Guid courseId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
            var result = await _assignmentService.GetAssignmentsByCourseIdAsync(courseId, userId);
            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpPost("api/assignments")]
        public async Task<IActionResult> CreateAssignment([FromForm] CreateAssignmentDTO assignmentDto)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (assignmentDto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _assignmentService.CreateAssignmentAsync(assignmentDto, userId);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpPut("api/assignments/{assignmentId}")]
        public async Task<IActionResult> UpdateAssignment(Guid assignmentId, [FromForm] UpdateAssignmentDTO assignmentDto)
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (assignmentDto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });


            var result = await _assignmentService.UpdateAssignmentAsync(assignmentId, assignmentDto, userId);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpDelete("api/assignments/{assignmentId}")]
        public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
            var result = await _assignmentService.DeleteAssignmentAsync(assignmentId, userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }

        [AuthorizeRoles(UserRoles.Student)]
        [HttpPost("api/assignments/submissions")]
        public async Task<IActionResult> SubmitAssignment([FromForm] SubmitAssignmentDTO submitAssignmentDTO)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
            var result = await _assignmentService.SubmitAssignmentAsync(submitAssignmentDTO, userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Student)]
        [HttpGet("api/assignments/{assignmentId}/submissions/student")]
        public async Task<IActionResult> GetMySubmissions(Guid assignmentId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
            var result = await _assignmentService.GetMySubmissionsAsync(assignmentId, userId);
            return Ok(result);


        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpPut("api/assignments/submissions/{submissionId}/grade")]
        public async Task<IActionResult> CorrectSubmission(Guid submissionId, [FromBody] CorrectAssignmentSubmissionDTO dto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (dto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _assignmentService.CorrectSubmissionAsync(submissionId, dto, userId);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpGet("api/assignments/{assignmentId}/submissions")]
        public async Task<IActionResult> GetAllSubmissions(Guid assignmentId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            var result = await _assignmentService.GetAllSubmissionsAsync(assignmentId, userId);
            
            if (!result.IsSuccess)
                return BadRequest(result);
                
            return Ok(result);
        }
    }
}
