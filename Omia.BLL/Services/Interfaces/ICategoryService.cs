using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CourseCategoryDTO>> GetCategoriesByCourseIdAsync(Guid courseId);

        Task<CourseCategoryDTO> GetCategoryByIdAsync(Guid categoryId);

        Task<CourseCategoryCountDTO> GetCategoryCountByCourseIdAsync(Guid courseId);

        Task<CategoryActionResponseDTO> CreateCategoryAsync(CreateCategoryRequestDTO categoryDto);

        Task<BaseResponseDTO> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDTO categoryDto);

        Task<BaseResponseDTO> DeleteCategoryAsync(Guid categoryId);

    }
}
