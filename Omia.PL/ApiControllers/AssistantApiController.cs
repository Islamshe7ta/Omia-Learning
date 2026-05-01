using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Assistant;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class AssistantApiController : ControllerBase
    {
        private readonly IAssistantService _assistantService;

        public AssistantApiController(IAssistantService assistantService)
        {
            _assistantService = assistantService;
        }

        [HttpPost("api/assistants")]
        [AuthorizeRoles(UserRoles.Institute,UserRoles.Teacher)]
        public async Task<IActionResult> CreateAssistant([FromBody] CreateAssistantDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var result = await _assistantService.CreateAssistantAsync(request, userId);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("api/assistants/{assistantId}")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> UpdateAssistant(Guid assistantId, [FromBody] EditAssistantDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var result = await _assistantService.UpdateAssistantAsync(assistantId, request, userId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("api/assistants/{assistantId}")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> DeleteAssistant(Guid assistantId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var result = await _assistantService.DeleteAssistantAsync(assistantId, userId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("api/assistants/{assistantId}")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> GetAssistant(Guid assistantId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var assistant = await _assistantService.GetAssistantByIdAsync(assistantId, userId);

            if (assistant == null) return NotFound(new { IsSuccess = false, Message = "Assistant not found" });
            return Ok(assistant);
        }

        [HttpGet("api/assistants")]
        [AuthorizeRoles(UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> GetAllAssistants(Guid? teacherId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            var assistants = await _assistantService.GetAllAssistantsAsync(userId, teacherId);
            if (assistants == null) return NotFound(new { IsSuccess = false, Message = "No assistants found" });
            return Ok(assistants);
        }
    }
}
