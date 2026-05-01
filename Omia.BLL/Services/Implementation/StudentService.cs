using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Student;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentResponseDTO> CreateStudentAsync(CreateStudentDTO request, Guid? teacherId, Guid? instituteId)
        {
            bool hasParentInfo = !string.IsNullOrWhiteSpace(request.ParentFullName) ||
                                 !string.IsNullOrWhiteSpace(request.ParentEmail) ||
                                 !string.IsNullOrWhiteSpace(request.ParentUsername) ||
                                 !string.IsNullOrWhiteSpace(request.ParentPhoneNumber) ||
                                 !string.IsNullOrWhiteSpace(request.ParentPassword);
            Parent parent = null;

            if (await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, Guid.Empty))
                return new StudentResponseDTO { IsSuccess = false, Message = "Student username already exists" };

            if (hasParentInfo)
            {
                if (await _unitOfWork.Users.IsUsernameExistsAsync(request.ParentUsername, Guid.Empty))
                    return new StudentResponseDTO { IsSuccess = false, Message = "Parent username already exists" };

                parent = _mapper.Map<Parent>(request);
                parent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.ParentPassword);
                parent.Status = AccountStatus.Active;
                await _unitOfWork.Parents.AddAsync(parent);
            }

            var student = _mapper.Map<Student>(request);
            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            student.Parent = parent; 
            student.TeacherId = teacherId;
            student.InstituteId = instituteId;
            student.Status = AccountStatus.Active;
            await _unitOfWork.Students.AddAsync(student);

            var enrollment = new CourseStudent
            {
                CourseId = request.CourseId,
                Student = student
            };
            await _unitOfWork.CourseStudents.AddAsync(enrollment);

            await _unitOfWork.CommitAsync();

            return new StudentResponseDTO
            {
                IsSuccess = true,
                Message = "Student created successfully",
                StudentId = student.Id
            };
        }

        public async Task<BaseResponseDTO> UpdateStudentAsync(Guid studentId, EditStudentDTO request, Guid? teacherId, Guid? instituteId)
        {
            var student = await _unitOfWork.Students.GetByIdAndContextAsync(studentId, teacherId, instituteId);
            if (student == null) return new BaseResponseDTO { IsSuccess = false, Message = "Student not found" };

            if (request.Username != student.Username && await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, student.Id))
                return new BaseResponseDTO { IsSuccess = false, Message = "Username already exists" };

            _mapper.Map(request, student);
            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Student updated successfully" };
        }

        public async Task<BaseResponseDTO> UpdateParentAsync(Guid studentId, EditParentDTO request, Guid? teacherId, Guid? instituteId)
        {
            var student = await _unitOfWork.Students.GetByIdAndContextAsync(studentId, teacherId, instituteId);
            if (student == null) return new BaseResponseDTO { IsSuccess = false, Message = "Student not found" };

            if (student.ParentId == null)
            {
                if (await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, Guid.Empty))
                    return new BaseResponseDTO { IsSuccess = false, Message = "Parent username already exists" };
                var parent = _mapper.Map<Parent>(request);
                parent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password ?? "Default123");
                parent.Status = AccountStatus.Active;
                await _unitOfWork.Parents.AddAsync(parent);
                student.Parent = parent;
                _unitOfWork.Students.Update(student);
                await _unitOfWork.CommitAsync();
                return new BaseResponseDTO { IsSuccess = true, Message = "Parent created and linked to student successfully" };
            }
            else
            {
                var parent = student.Parent!;
                if (request.Username != parent.Username && await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, parent.Id))
                    return new BaseResponseDTO { IsSuccess = false, Message = "Parent username already exists" };

                _mapper.Map(request, parent);
                _unitOfWork.Parents.Update(parent);
                await _unitOfWork.CommitAsync();

                return new BaseResponseDTO { IsSuccess = true, Message = "Parent updated successfully" };
            }
        }

        public async Task<BaseResponseDTO> DeleteStudentAsync(Guid studentId, Guid? teacherId, Guid? instituteId)
        {
            var student = await _unitOfWork.Students.GetByIdAndContextAsync(studentId, teacherId, instituteId);
            if (student == null) return new BaseResponseDTO { IsSuccess = false, Message = "Student not found" };

            student.IsDeleted = true;
            student.Status = AccountStatus.Deleted;
            _unitOfWork.Students.Update(student);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Student deleted successfully" };
        }

        public async Task<StudentProfileDTO> GetStudentByIdAsync(Guid studentId, Guid? teacherId, Guid? instituteId)
        {
            var student = await _unitOfWork.Students.GetByIdAndContextAsync(studentId, teacherId, instituteId);
            if (student == null) return null;

            return _mapper.Map<StudentProfileDTO>(student);
        }

        public async Task<IEnumerable<StudentBriefDTO>> GetAllStudentsAsync(Guid? teacherId, Guid? instituteId)
        {
            var students = await _unitOfWork.Students.GetAllByContextAsync(teacherId, instituteId);
            return _mapper.Map<IEnumerable<StudentBriefDTO>>(students);
        }

        public async Task<IEnumerable<StudentBriefDTO>> GetStudentsByCourseIdAsync(Guid courseId)
        {
            var students = await _unitOfWork.Students.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<StudentBriefDTO>>(students);
        }
    }
}
