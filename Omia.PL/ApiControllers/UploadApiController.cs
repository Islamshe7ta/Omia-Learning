using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.Upload;
using Omia.BLL.Services.Interfaces;

namespace Omia.PL.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UploadApiController : ControllerBase
    {
        private readonly IFileService _fileService;

        public UploadApiController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadRequestDTO request)
        {
            if (request == null || request.File == null)
            {
                return BadRequest(new UploadResponseDTO 
                { 
                    IsSuccess = false, 
                    Message = "No file provided" 
                });
            }

            var result = await _fileService.UploadFileAsync(request.File, request.Path, request.Name);

            if (!result.IsSuccess)
            {
                return BadRequest(new UploadResponseDTO
                {
                    IsSuccess = false,
                    Message = result.Message
                });
            }

            return Ok(new UploadResponseDTO
            {
                IsSuccess = true,
                Message = "File uploaded successfully",
                Path = result.FileUrl ?? string.Empty
            });
        }
    }
}
