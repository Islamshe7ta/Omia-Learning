using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.AssistantCourse;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class AssistantCourseService : IAssistantCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssistantCourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseDTO> AssignAssistantToCourseAsync(AssignAssistantToCourseDTO request)
        {
            // 1. Check Course existence
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Course not found" };

            // 2. Check Assistant existence
            var assistant = await _unitOfWork.Assistants.GetByIdAsync(request.AssistantId);
            if (assistant == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Assistant not found" };

            // 3. Check if already assigned
            if (await _unitOfWork.AssistantCourses.IsAssistantAssignedAsync(request.AssistantId, request.CourseId))
                return new BaseResponseDTO { IsSuccess = false, Message = "Assistant already assigned to this course" };

            // 4. Assign using Mapper
            var assignment = _mapper.Map<AssistantCourse>(request);

            await _unitOfWork.AssistantCourses.AddAsync(assignment);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO
            {
                IsSuccess = true,
                Message = "Assistant assigned to course successfully"
            };
        }

        public async Task<BaseResponseDTO> RemoveAssistantFromCourseAsync(Guid assistantId, Guid courseId)
        {
            var assignment = await _unitOfWork.AssistantCourses.GetAssignmentAsync(assistantId, courseId);
            if (assignment == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Assistant not assigned to this course" };

            _unitOfWork.AssistantCourses.Delete(assignment);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO
            {
                IsSuccess = true,
                Message = "Assistant removed from course successfully"
            };
        }

        public async Task<BaseResponseDTO> UpdateAssistantRolesAsync(UpdateAssistantRolesDTO request)
        {
            var assignment = await _unitOfWork.AssistantCourses.GetAssignmentAsync(request.AssistantId, request.CourseId);
            if (assignment == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Assistant not assigned to this course" };

            // Use Mapper to update existing entity
            _mapper.Map(request, assignment);
            
            _unitOfWork.AssistantCourses.Update(assignment);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO
            {
                IsSuccess = true,
                Message = "Assistant roles updated successfully"
            };
        }
    }
}
