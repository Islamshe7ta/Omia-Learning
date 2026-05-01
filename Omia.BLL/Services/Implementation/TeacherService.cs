using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.DTOs.Teacher;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TeacherCreatedResponseDTO> CreateTeacherAsync(CreateTeacherDTO request, Guid? instituteId)
        {
            bool exists = await _unitOfWork.Teachers.IsUsernameUsedAsync(request.Username);
            if (exists)
            {
                return new TeacherCreatedResponseDTO 
                { 
                    IsSuccess = false, 
                    Message = "Username already exists" 
                };
            }

            var teacher = _mapper.Map<Teacher>(request);
            
            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            teacher.InstituteId = instituteId;
            teacher.Status = AccountStatus.Active;

            await _unitOfWork.Teachers.AddAsync(teacher);
            await _unitOfWork.CommitAsync();

            return new TeacherCreatedResponseDTO
            {
                IsSuccess = true,
                Message = "Teacher created successfully",
                TeacherId = teacher.Id
            };
        }

        public async Task<BaseResponseDTO> UpdateTeacherAsync(Guid teacherId, EditTeacherDTO request, Guid? instituteId)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAndInstituteIdAsync(teacherId, instituteId);
            if (teacher == null) return new BaseResponseDTO { IsSuccess = false, Message = "Teacher not found" };

            if (request.Username != teacher.Username)
            {
                bool exists = await _unitOfWork.Teachers.IsUsernameUsedAsync(request.Username, teacher.Id);
                if (exists) return new BaseResponseDTO { IsSuccess = false, Message = "Username already exists" };
            }

            _mapper.Map(request, teacher);
            
            _unitOfWork.Teachers.Update(teacher);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Teacher updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteTeacherAsync(Guid teacherId, Guid? instituteId)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAndInstituteIdAsync(teacherId, instituteId);
            if (teacher == null) return new BaseResponseDTO { IsSuccess = false, Message = "Teacher not found" };

            teacher.IsDeleted = true;
            teacher.Status = AccountStatus.Deleted;
            _unitOfWork.Teachers.Update(teacher);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Teacher deleted successfully" };
        }

        public async Task<TeacherProfileDTO> GetTeacherByIdAsync(Guid teacherId, Guid? instituteId)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAndInstituteIdAsync(teacherId, instituteId);
            if (teacher == null) return null;

            return _mapper.Map<TeacherProfileDTO>(teacher);
        }

        public async Task<IEnumerable<TeacherProfileDTO>> GetAllTeachersAsync(Guid? instituteId)
        {
            var teachers = await _unitOfWork.Teachers.GetAllTeachersByContextAsync(instituteId);
            return _mapper.Map<IEnumerable<TeacherProfileDTO>>(teachers);
        }
    }
}
