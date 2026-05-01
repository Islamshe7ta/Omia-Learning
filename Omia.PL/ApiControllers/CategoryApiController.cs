using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omia.BLL.DTOs.CourseCategory;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Models.Enums;
using Omia.PL.Middlewares;

namespace Omia.PL.ApiControllers
{
    /// Agnefits: This controller actions has to manage user authorization and access to course!!
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("api/categories")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Category is null");
            var result = await _categoryService.CreateCategoryAsync(categoryDto);
            return Ok(result);
        }

        [HttpPut("api/categories/{categoryId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] UpdateCategoryRequestDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("بيانات القسم غير مكتملة.");

            var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("api/categories/{categoryId}")]
        [AuthorizeRoles(UserRoles.Teacher, UserRoles.Assistant)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var category = await _categoryService.DeleteCategoryAsync(categoryId);
            if (!category.IsSuccess)
                return BadRequest(category);
            return Ok(category);
        }

        [HttpGet("api/courses/{courseId}/categories")]
        [Authorize]
        public async Task<IActionResult> GetCategoriesByCourseId(Guid courseId)
        {
            var categories = await _categoryService.GetCategoriesByCourseIdAsync(courseId);
            if (categories == null)
                return NotFound(categories);
            return Ok(categories);
        }

        [HttpGet("api/categories/{categoryId}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return NotFound(category);
            return Ok(category);
        }

        [HttpGet("api/courses/{courseId}/categories/count")]
        [Authorize]
        public async Task<IActionResult> GetCategoryCountByCourseId(Guid courseId)
        {
            var count = await _categoryService.GetCategoryCountByCourseIdAsync(courseId);
            if (count == null)
                return NotFound(count);
            return Ok(count);
        }
    }
}
