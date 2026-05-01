using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Teacher;
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
    public class TeacherApiController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherApiController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("api/teachers")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute)]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDTO request)
        {
            if (request == null) return BadRequest("Teacher data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);
            
            var result = await _teacherService.CreateTeacherAsync(request, context.InstituteId);
            
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("api/teachers/{teacherId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute)]
        public async Task<IActionResult> UpdateTeacher(Guid teacherId, [FromBody] EditTeacherDTO request)
        {
            if (request == null) return BadRequest("Teacher data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _teacherService.UpdateTeacherAsync(teacherId, request, context.InstituteId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("api/teachers/{teacherId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute)]
        public async Task<IActionResult> DeleteTeacher(Guid teacherId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var result = await _teacherService.DeleteTeacherAsync(teacherId, context.InstituteId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("api/teachers/{teacherId}")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute)]
        public async Task<IActionResult> GetTeacher(Guid teacherId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var teacher = await _teacherService.GetTeacherByIdAsync(teacherId, context.InstituteId);

            if (teacher == null) return NotFound(new { IsSuccess = false, Message = "Teacher not found" });
            return Ok(teacher);
        }

        [HttpGet("api/teachers")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute)]
        public async Task<IActionResult> GetAllTeachers()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest("Invalid user ID format");

            var context = UserRoleHelper.ResolveContext(userId, User.FindFirst(ClaimTypes.Role)?.Value);

            var teachers = await _teacherService.GetAllTeachersAsync(context.InstituteId);
            return Ok(teachers);
        }
    }
}
