using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.Services.Interfaces;

namespace Omia.PL.ApiControllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthApiController(IAuthService authService )
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.Login(request.UserName, request.Password);

            if (!result.IsSuccess)
                return Unauthorized(result); 


            return Ok(result);
        }

    }

}
