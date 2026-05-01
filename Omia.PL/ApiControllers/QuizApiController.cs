using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Quiz;
using Omia.BLL.DTOs.QuizAttempt;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;

namespace Omia.PL.ApiControllers
{
    [ApiController]
    public class QuizApiController : ControllerBase
    {
        private readonly IQuizService _quizService;
        public QuizApiController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("api/quizzes/{quizId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> GetQuizDetails(Guid quizId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            var result = await _quizService.GetQuizDetailsAsync(quizId, userId);

            if (!result.IsSuccess)
                return StatusCode(403, result);

            return Ok(result);
        }

        [HttpGet("api/quizzes/course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetQuizzesByCourse(Guid courseId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            var result = await _quizService.GetQuizzesByCourseIdAsync(courseId, userId);

            return Ok(result);
        }
      
        [HttpPut("api/quizzes/{quizId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> UpdateQuiz(Guid quizId, [FromBody] UpdateQuizDTO quizDto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (quizDto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _quizService.UpdateQuizAsync(quizId, quizDto, userId);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }

        [HttpDelete("api/quizzes/{quizId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> DeleteQuiz(Guid quizId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            var result = await _quizService.DeleteQuizAsync(quizId, userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("api/quizzes")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO quizDto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (quizDto == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _quizService.CreateQuizAsync(quizDto, userId);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }
        [HttpPost("api/coursequiz/start")]
        [AuthorizeRoles(UserRoles.Student)]
        public async Task<IActionResult> StartQuiz([FromBody] StartQuizRequestDTO request)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (request == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _quizService.StartQuizAsync(request, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("api/coursequiz/end")]
        [AuthorizeRoles(UserRoles.Student)]
        public async Task<IActionResult> EndQuiz([FromBody] EndQuizRequestDTO request)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            if (request == null)
                return BadRequest(new { isSuccess = false, message = "Request body cannot be empty" });

            var result = await _quizService.EndQuizAsync(request, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
 
        [HttpGet("api/quizzes/{quizId}/attempts/student")]
        [AuthorizeRoles(UserRoles.Student)]
        public async Task<IActionResult> GetMyAttempts(Guid quizId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
 
            var result = await _quizService.GetMyAttemptsAsync(quizId, userId);
            return Ok(result);
        }
 
        [HttpGet("api/quizzes/{quizId}/attempts")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> GetQuizAttempts(Guid quizId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });
 
            var result = await _quizService.GetQuizAttemptsForTeacherAsync(quizId, userId);
 
            if (!result.IsSuccess)
                return BadRequest(result);
 
            return Ok(result);
        }

        [HttpGet("api/quizzes/attempts/{attemptId}")]
        [Authorize]
        public async Task<IActionResult> GetAttemptDetails(Guid attemptId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { isSuccess = false, message = "Invalid or missing User ID in token" });

            var result = await _quizService.GetQuizAttemptDetailsAsync(attemptId, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
