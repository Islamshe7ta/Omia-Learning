using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Profile;
using Omia.BLL.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Omia.PL.ApiControllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileApiController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileApiController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.GetProfileAsync(userId);

            if (!result.IsSuccess)
            {
                return NotFound(result); 
            }

            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdateProfileAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("fullname")]
        public async Task<IActionResult> UpdateFullname([FromBody] UpdateFullnameDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdateFullnameAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdateUsernameAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdateEmailAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("phonenumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody] UpdatePhoneNumberDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdatePhoneNumberAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("location")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateLocationDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.UpdateLocationAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            var result = await _profileService.ChangePasswordAsync(userId, request);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile image)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized(new { isSuccess = false, message = "Invalid token or user ID not found." });
            }

            if (image == null || image.Length == 0)
            {
                return BadRequest(new { isSuccess = false, message = "Please provide an image file." });
            }

            using var stream = image.OpenReadStream();
            var uploadRequest = new ProfileImageUploadRequest
            {
                FileStream = stream,
                FileName = image.FileName,
                ContentType = image.ContentType,
                Length = image.Length
            };

            var result = await _profileService.UpdateProfileImageAsync(userId, uploadRequest);

            if (!result.IsSuccess)
            {
                if (result.Message == "User not found") return NotFound(result);
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
