using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.AssistantCourse;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using System.Threading.Tasks;

namespace Omia.PL.ApiControllers
{
    /// Agnefits: This controller actions has to manage user authorization and access to course and assistant!!
    [ApiController]
    public class CourseAssistantApiController : ControllerBase
    {
        private readonly IAssistantCourseService _assistantCourseService;

        public CourseAssistantApiController(IAssistantCourseService assistantCourseService)
        {
            _assistantCourseService = assistantCourseService;
        }

        [HttpPost("api/course-assistants")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> AssignAssistant([FromBody] AssignAssistantToCourseDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var result = await _assistantCourseService.AssignAssistantToCourseAsync(request);
            
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("api/course-assistants")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> RemoveAssistant([FromBody] RemoveAssistantFromCourseDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var result = await _assistantCourseService.RemoveAssistantFromCourseAsync(request.AssistantId, request.CourseId);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("api/course-assistants/roles")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> UpdateRoles([FromBody] UpdateAssistantRolesDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var result = await _assistantCourseService.UpdateAssistantRolesAsync(request);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
