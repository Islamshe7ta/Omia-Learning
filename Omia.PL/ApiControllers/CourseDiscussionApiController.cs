using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.CourseDiscussion;
using Omia.BLL.Services.Interfaces;
using System.Security.Claims;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class CourseDiscussionApiController : ControllerBase
    {
        private readonly ICourseDiscussionService _courseDiscussionService;

        public CourseDiscussionApiController(ICourseDiscussionService courseDiscussionService)
        {
            _courseDiscussionService = courseDiscussionService;
        }

        [Authorize]
        [HttpPost("/api/course-discussions")]
        public async Task<IActionResult> SendMessage([FromBody] CreateDiscussionMessageDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            if (dto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be null" });

            var result = await _courseDiscussionService.SendMessageAsync(dto, userId);

            if (!result.IsSuccess)
            {
                if (result.Message == "Student not enrolled in this course")
                    return Unauthorized(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("/api/courses/{courseId}/discussions")]
        public async Task<IActionResult> GetCourseDiscussions(Guid courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            var result = await _courseDiscussionService.GetCourseDiscussionsAsync(courseId, userId);

            if (result == null || !result.Any())
                return NotFound(new { isSuccess = false, message = "No discussions found for this course or you don't have access" });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("/api/chat")]
        public async Task<IActionResult> GetChatHome()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            var result = await _courseDiscussionService.GetChatHomeAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("/api/chat/{otherUserId}")]
        public async Task<IActionResult> GetPrivateChat(Guid otherUserId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid currentUserId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            var result = await _courseDiscussionService.GetPrivateChatAsync(currentUserId, otherUserId);
            return Ok(result);
        }
    }
}
