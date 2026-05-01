using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Course;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;

namespace Omia.PL.ApiControllers
{
    /// Agnefits: This controller actions has to manage user authorization and access to course and check limits!!
    [ApiController]
    public class CoursesApiController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesApiController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost("api/courses")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> CreateCourse([FromForm] CreateCourseDTO courseDto)
        {
            if (courseDto == null) return BadRequest("Course is null");
            var result = await _courseService.CreateCourseAsync(courseDto);
            return Ok(result);
        }

        [HttpPut("api/courses/{courseId}")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> UpdateCourse(Guid courseId, [FromForm] CreateCourseDTO courseDto)
        {
            if (courseDto == null) return BadRequest("Course is null");
            var result = await _courseService.UpdateCourseAsync(courseId, courseDto);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("api/courses/{courseId}")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var result = await _courseService.DeleteCourseAsync(courseId);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("api/courses")]
        //[Authorize(Roles = "Student")]
        [Authorize]
        public async Task<IActionResult> GetMyCourses()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = Guid.Parse(userIdClaim);

            var result = await _courseService.GetCoursesAsync(user);
            return Ok(result);
        }

        [HttpGet("api/courses/{courseId}/details")]
        //[Authorize(Roles = "Student")]
        [Authorize]
        public async Task<IActionResult> GetCourseDetailsFull(Guid courseId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdClaim);

            var result = await _courseService.GetCourseDetailsFullAsync(courseId, userId);

            if (result.StatusCode == 404) return NotFound(result);
            if (result.StatusCode == 403) return StatusCode(403, result);

            return Ok(result);
        }

        [HttpGet("api/courses/{courseId}/brief")]
        [Authorize]
        public async Task<IActionResult> GetCourseBrief(Guid courseId)
        {
            var result = await _courseService.GetCourseBriefAsync(courseId);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("api/courses/{courseId}/notifications")]
        //[Authorize(Roles = "Student")]
        [Authorize]
        public async Task<IActionResult> GetActivityFeed(Guid courseId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var result = await _courseService.GetActivityFeedAsync(courseId, userId);
            return Ok(result);
        }
    }
}
