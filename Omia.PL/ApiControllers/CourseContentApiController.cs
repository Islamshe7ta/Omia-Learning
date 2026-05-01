using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseContent;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using System.Security.Claims;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class CourseContentApiController : ControllerBase
    {
        private readonly ICourseContentService _courseContentService;

        public CourseContentApiController(ICourseContentService courseContentService)
        {
            _courseContentService = courseContentService;
        }

        [HttpPost("api/course-content")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> CreateCourseContent([FromForm] CreateCourseContentDTO createCourseContentDTO)
        {
            if (createCourseContentDTO == null)
                return BadRequest("Course content data is required");


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out Guid uploaderId))
                return BadRequest("Invalid user ID format");

            var result = await _courseContentService.CreateCourseContentAsync(createCourseContentDTO, uploaderId);

            if (!result.IsSuccess)
            {
                if (result.Message == "Course not found")
                    return NotFound(result);
                if (result.Message == "Category not found")
                    return NotFound(result);
                if (result.Message == "You are not allowed to create content")
                    return Forbid(result.Message);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("api/course-content/{courseContentId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> UpdateCourseContent(Guid courseContentId, [FromForm] UpdateCourseContentDTO updateCourseContentDTO)
        {
            if (updateCourseContentDTO == null)
                return BadRequest("Course content data is required");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var uploaderId))
                return BadRequest("Invalid user ID format");

            var result = await _courseContentService.UpdateCourseContentAsync(
                updateCourseContentDTO,
                courseContentId,
                uploaderId
            );

            if (!result.IsSuccess)
            {
                if (result.Message == "Course content not found")
                    return NotFound(result);
                if (result.Message == "Category not found")
                    return NotFound(result);
                if (result.Message == "You are not allowed to edit content")
                    return Forbid(result.Message);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("api/course-content/{courseContentId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> DeleteCourseContent(Guid courseContentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var result = await _courseContentService.DeleteCourseContentAsync(courseContentId, userId);

            if (!result.IsSuccess)
            {
                if (result.Message == "Course content not found")
                    return NotFound(result);
                if (result.Message == "You are not allowed to delete content")
                    return Forbid(result.Message);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("api/course-content/{contentId}")]
        [AuthorizeRoles(UserRoles.Student, UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> GetCourseContentDetails(Guid contentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var result = await _courseContentService.GetCourseContentDetailsAsync(contentId, userId);

            if (!result.IsSuccess)
            {
                if (result.Message == "Content not found")
                    return NotFound(result);
                if (result.Message == "You are not allowed to view this content")
                    return StatusCode(403, result);
                
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpPost("api/course-content/comments")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] AddContentCommentDTO addCommentDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var result = await _courseContentService.AddCommentAsync(addCommentDTO, userId);
            if (!result.IsSuccess) {
                if (result.Message == "Content not found")
                    return NotFound(result);
                if (result.Message == "You are not allowed to comment on this content" || result.Message == "Student not enrolled in this course")
                    return StatusCode(403, result);
                if(string.IsNullOrWhiteSpace(result.Message))
                    return BadRequest("Message is required");
                return BadRequest(result);
            }
            return Ok(result);

        }

        [HttpGet("api/courses/{courseId}/course-content/count")]
        [Authorize]
        public async Task<IActionResult> GetCourseContentCount(Guid courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var count = await _courseContentService.GetCourseContentCountAsync(courseId, userId);
            if (count == null)
                return NotFound(count);
            return Ok(count);
        }

        [HttpGet("api/courses/{courseId}/course-content/newest")]
        [Authorize]
        public async Task<IActionResult> GetNewestVideos(Guid courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var newestVideos = await _courseContentService.GetNewestVideosAsync(courseId, userId); 
            return Ok(newestVideos);
        }

        [HttpGet("api/courses/{courseId}/course-content")]
        [Authorize]
        public async Task<IActionResult> GetCourseContents(Guid courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var contents = await _courseContentService.GetCourseContentsAsync(courseId, userId);
            
            if (contents == null) 
                return StatusCode(403, new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to view this course's content" });

            return Ok(contents); 
        }

        [HttpGet("api/categories/{categoryId}/course-content")]
        [Authorize]
        public async Task<IActionResult> GetCategoryContents(Guid categoryId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            var contents = await _courseContentService.GetCategoryContentsAsync(categoryId, userId);

            if (contents == null) 
                return StatusCode(403, new BaseResponseDTO { IsSuccess = false, Message = "Category not found or you are not allowed to view its content" });

            return Ok(contents); 
        }

    }
}