using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseCategory;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;



namespace Omia.BLL.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryActionResponseDTO> CreateCategoryAsync(CreateCategoryRequestDTO categoryDto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(categoryDto.CourseId);
            if (course == null)
            {
                return new CategoryActionResponseDTO
                {
                    IsSuccess = false,
                    Message = "عفواً، الكورس ده غير موجود."
                };
            }
            var category = _mapper.Map<CourseCategory>(categoryDto);
            
            if (!categoryDto.OrderNumber.HasValue)
            {
                var maxOrder = await _unitOfWork.CourseCategories.MaxAsync(c => c.CourseId == categoryDto.CourseId, c => c.OrderNumber);
                category.OrderNumber = maxOrder + 1;
            }

            await _unitOfWork.CourseCategories.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return new CategoryActionResponseDTO
            {
                IsSuccess = true,
                Message = "تم انشاء القسم بنجاح.",
                CategoryId = category.Id

            };

        }

        public async Task<BaseResponseDTO> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _unitOfWork.CourseCategories.GetByIdAsync(categoryId);

            if (category == null)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "عفواً، القسم ده غير موجود." };
            }

            _unitOfWork.CourseCategories.Delete(category);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "تم حذف القسم بنجاح." };
        }


        public async Task<IEnumerable<CourseCategoryDTO>> GetCategoriesByCourseIdAsync(Guid courseId)
        {
            var categories = await _unitOfWork.CourseCategories.GetCategoriesByCourseId(courseId);
            return _mapper.Map<IEnumerable<CourseCategoryDTO>>(categories);
        }

        public async Task<CourseCategoryDTO> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _unitOfWork.CourseCategories.GetByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            return _mapper.Map<CourseCategoryDTO>(category);

        }

        public async Task<CourseCategoryCountDTO> GetCategoryCountByCourseIdAsync(Guid courseId)
        {

            var course = await _unitOfWork.Courses.GetCourseBriefAsync(courseId);

            if (course == null)
                return null;

            return _mapper.Map<CourseCategoryCountDTO>(course);
        }


        public async Task<BaseResponseDTO> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDTO categoryDto)
        {
            var category = await _unitOfWork.CourseCategories.GetByIdAsync(categoryId);
            if (category == null)
            {
                return new BaseResponseDTO { IsSuccess = false, Message = "عفواً، القسم هذا غير موجود." };
            }
            var originalOrder = category.OrderNumber;
            _mapper.Map(categoryDto, category);

            if (!categoryDto.OrderNumber.HasValue)
            {
                category.OrderNumber = originalOrder;
            }

            _unitOfWork.CourseCategories.Update(category);
            await _unitOfWork.CommitAsync();
            return new BaseResponseDTO { IsSuccess = true, Message = "تم تحديث القسم بنجاح." };

        }
    }
}
