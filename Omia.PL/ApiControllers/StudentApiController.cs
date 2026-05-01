using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Student;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using Omia.BLL.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentApiController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("api/students")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO request)
        {
            if (request == null) return BadRequest("Student data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _studentService.CreateStudentAsync(request, context.TeacherId, context.InstituteId);
            
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("api/students/{studentId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> UpdateStudent(Guid studentId, [FromBody] EditStudentDTO request)
        {
            if (request == null) return BadRequest("Student data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _studentService.UpdateStudentAsync(studentId, request, context.TeacherId, context.InstituteId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("api/students/{studentId}/parent")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> UpdateParent(Guid studentId, [FromBody] EditParentDTO request)
        {
            if (request == null) return BadRequest("Parent data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _studentService.UpdateParentAsync(studentId, request, context.TeacherId, context.InstituteId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("api/students/{studentId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _studentService.DeleteStudentAsync(studentId, context.TeacherId, context.InstituteId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("api/students/{studentId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> GetStudent(Guid studentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var student = await _studentService.GetStudentByIdAsync(studentId, context.TeacherId, context.InstituteId);

            if (student == null) return NotFound(new { IsSuccess = false, Message = "Student not found" });
            return Ok(student);
        }

        [HttpGet("api/students")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> GetAllStudents()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var students = await _studentService.GetAllStudentsAsync(context.TeacherId, context.InstituteId);
            return Ok(new { IsSuccess = true, Students = students });
        }

        [HttpGet("api/courses/{courseId}/students")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> GetStudentsByCourse(Guid courseId)
        {
            var students = await _studentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(new { IsSuccess = true, Students = students });
        }
    }
}
