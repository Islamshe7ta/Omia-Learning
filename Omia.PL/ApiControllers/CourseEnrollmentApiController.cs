using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.CourseEnrollment;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;
using System.Threading.Tasks;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class CourseEnrollmentApiController : ControllerBase
    {
        private readonly ICourseEnrollmentService _enrollmentService;

        public CourseEnrollmentApiController(ICourseEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost("api/course-enrollments")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> AssignStudentToCourse([FromBody] AssignStudentToCourseDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var result = await _enrollmentService.AssignStudentToCourseAsync(request);
            
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("api/course-enrollments")]
        [AuthorizeRoles(UserRoles.Admin, UserRoles.Institute, UserRoles.Teacher)]
        public async Task<IActionResult> RemoveStudentFromCourse([FromBody] RemoveStudentFromCourseDTO request)
        {
            if (request == null) return BadRequest("Invalid data");

            var result = await _enrollmentService.RemoveStudentFromCourseAsync(request);

            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
