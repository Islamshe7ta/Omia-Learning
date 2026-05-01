using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseEnrollment;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class CourseEnrollmentService : ICourseEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseEnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnrollmentResponseDTO> AssignStudentToCourseAsync(AssignStudentToCourseDTO request)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                return new EnrollmentResponseDTO { IsSuccess = false, Message = "Course not found" };

            var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
            if (student == null)
                return new EnrollmentResponseDTO { IsSuccess = false, Message = "Student not found" };

            if (await _unitOfWork.CourseStudents.IsStudentEnrolledInCourseAsync(request.StudentId, request.CourseId))
                return new EnrollmentResponseDTO { IsSuccess = false, Message = "Student already enrolled in this course" };

            var enrollment = _mapper.Map<CourseStudent>(request);

            await _unitOfWork.CourseStudents.AddAsync(enrollment);
            await _unitOfWork.CommitAsync();

            return new EnrollmentResponseDTO
            {
                IsSuccess = true,
                Message = "Student assigned to course successfully",
                EnrollmentId = enrollment.Id,
                EnrollmentDate = enrollment.CreatedAt
            };
        }

        public async Task<BaseResponseDTO> RemoveStudentFromCourseAsync(RemoveStudentFromCourseDTO request)
        {
            var enrollment = await _unitOfWork.CourseStudents.GetEnrollmentAsync(request.StudentId, request.CourseId);
            if (enrollment == null)
                return new BaseResponseDTO { IsSuccess = false, Message = "Student not enrolled in this course" };

            _unitOfWork.CourseStudents.Delete(enrollment);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO
            {
                IsSuccess = true,
                Message = "Student removed from course successfully"
            };
        }
    }
}
