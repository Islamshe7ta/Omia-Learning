using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.LiveSession;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using System.Security.Claims;

namespace Omia.PL.ApiControllers
{

    [ApiController]
    public class LiveSessionApiController : ControllerBase
    {
        private readonly ILiveSessionService _ILiveSessionService;

        public LiveSessionApiController(ILiveSessionService ILiveSessionService)
        {
            _ILiveSessionService = ILiveSessionService;
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpPost("/api/live-sessions")]
        public async Task<IActionResult> CreateLiveSession([FromBody] CreateLiveSessionDTO liveSessionDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid managerId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            if (liveSessionDto == null)
                return BadRequest(new { isSuccess = false, message = "Live session is null" });

            var result = await _ILiveSessionService.CreateLiveSessionAsync(liveSessionDto, managerId);


            if (!result.IsSuccess)
            {
                if (result.Message == "You are not allowed to create live sessions")
                    return Unauthorized(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpPut("/api/live-sessions/{liveSessionId}")]
        public async Task<IActionResult> UpdateLiveSession(Guid liveSessionId, [FromBody] UpdateLiveSessionDTO liveSessionDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid managerId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            if (liveSessionDto == null)
                return BadRequest(new { isSuccess = false, message = "Live session is null" });

            var result = await _ILiveSessionService.UpdateLiveSessionAsync(liveSessionId, liveSessionDto, managerId);

            if (!result.IsSuccess)
            {
                if (result.Message == "You are not allowed to update live sessions")
                    return Unauthorized(result);

                return BadRequest(result);
            }
            return Ok(result);
        }

        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        [HttpDelete("/api/live-sessions/{liveSessionId}")]
        public async Task<IActionResult> DeleteLiveSession(Guid liveSessionId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });

            if (!Guid.TryParse(userIdClaim, out Guid managerId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });

            var result = await _ILiveSessionService.DeleteLiveSessionAsync(liveSessionId, managerId);

            if (!result.IsSuccess)
            {
                if (result.Message == "You are not allowed to delete live sessions")
                    return Unauthorized(result);

                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize]
        [HttpGet("/api/courses/{courseId}/live-sessions")]
        public async Task<IActionResult> GetLiveSessionsByCourseId(Guid courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { isSuccess = false, message = "User ID not found in token" });
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return BadRequest(new { isSuccess = false, message = "Invalid user ID format" });
            var result = await _ILiveSessionService.GetLiveSessionsByCourseAsync(courseId, userId);
            if (result == null || !result.Any())
                return NotFound(new { isSuccess = false, message = "No live sessions found for this course or you don't have access" });
            return Ok(result);
        }
    }
}
