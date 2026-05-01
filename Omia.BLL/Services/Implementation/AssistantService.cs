using AutoMapper;
using Omia.BLL.DTOs.Assistant;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Auth;

namespace Omia.BLL.Services.Implementation
{
    public class AssistantService : IAssistantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssistantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssistantCreatedResponseDTO> CreateAssistantAsync(CreateAssistantDTO request, Guid userId)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(userId);
            if (teacher == null || teacher.IsDeleted || teacher.Status != AccountStatus.Active)
            {
                return new AssistantCreatedResponseDTO
                {
                    IsSuccess = false,
                    Message = "Teacher not found or you don't have permission to create an assistant"
                };
            }
            else if (request.TeacherId.HasValue && request.TeacherId.Value != userId)
            {
                var institute = await _unitOfWork.Institutes.GetByIdAsync(userId);
                if (institute == null || institute.IsDeleted || institute.Status != AccountStatus.Active)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Institute not found or you don't have permission to create an assistant"
                    };
                }
                teacher = await _unitOfWork.Teachers.GetByIdAsync(request.TeacherId.Value);
                if (teacher == null || teacher.IsDeleted || teacher.InstituteId != institute.Id)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Teacher not found or you don't have permission to edit"
                    };
                }
            }
            else
            {
                return new AssistantCreatedResponseDTO
                {
                    IsSuccess = false,
                    Message = "You must specify a valid teacher ID to create an assistant"
                };

            }


            bool exists = await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, Guid.Empty);
            if (exists)
            {
                return new AssistantCreatedResponseDTO
                {
                    IsSuccess = false,
                    Message = "Username already exists"
                };
            }

            var assistant = _mapper.Map<Assistant>(request);

            assistant.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            assistant.TeacherId = teacher.Id;
            assistant.Status = AccountStatus.Active;

            await _unitOfWork.Assistants.AddAsync(assistant);
            await _unitOfWork.CommitAsync();

            return new AssistantCreatedResponseDTO
            {
                IsSuccess = true,
                Message = "Assistant created successfully",
                AssistantId = assistant.Id
            };
        }

        public async Task<BaseResponseDTO> UpdateAssistantAsync(Guid assistantId, EditAssistantDTO request, Guid userId)
        {
            var assistant = await _unitOfWork.Assistants.GetByIdAsync(assistantId);
            if (assistant == null) return new BaseResponseDTO { IsSuccess = false, Message = "Assistant not found or you don't have permission to edit" };

            var teacher = await _unitOfWork.Teachers.GetByIdAsync(userId);
            if (teacher == null || teacher.IsDeleted || teacher.Status != AccountStatus.Active || assistant.TeacherId != teacher.Id)
            {
                return new AssistantCreatedResponseDTO
                {
                    IsSuccess = false,
                    Message = "Teacher not found or you don't have permission to edit this assistant"
                };
            }
            else
            {
                var institute = await _unitOfWork.Institutes.GetByIdAsync(userId);
                if (institute == null || institute.IsDeleted || institute.Status != AccountStatus.Active)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Institute not found or you don't have permission to edit this assistant"
                    };
                }
                teacher = await _unitOfWork.Teachers.GetByIdAsync(assistant.TeacherId);
                if (teacher == null || teacher.IsDeleted || teacher.InstituteId != institute.Id || assistant.TeacherId != teacher.Id)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Teacher not found or you don't have permission to edit"
                    };
                }
            }

            if (request.Username != assistant.Username)
            {
                bool exists = await _unitOfWork.Users.IsUsernameExistsAsync(request.Username, assistant.Id);
                if (exists) return new BaseResponseDTO { IsSuccess = false, Message = "Username already exists" };
            }

            _mapper.Map(request, assistant);

            _unitOfWork.Assistants.Update(assistant);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Assistant updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteAssistantAsync(Guid assistantId, Guid userId)
        {
            var assistant = await _unitOfWork.Assistants.GetByIdAsync(assistantId);
            if (assistant == null) return new BaseResponseDTO { IsSuccess = false, Message = "Assistant not found or you don't have permission to delete" };

            var teacher = await _unitOfWork.Teachers.GetByIdAsync(userId);
            if (teacher == null || teacher.IsDeleted || teacher.Status != AccountStatus.Active || assistant.TeacherId != teacher.Id)
            {
                return new AssistantCreatedResponseDTO
                {
                    IsSuccess = false,
                    Message = "Teacher not found or you don't have permission to delete this assistant"
                };
            }
            else
            {
                var institute = await _unitOfWork.Institutes.GetByIdAsync(userId);
                if (institute == null || institute.IsDeleted || institute.Status != AccountStatus.Active)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Institute not found or you don't have permission to delete this assistant"
                    };
                }
                teacher = await _unitOfWork.Teachers.GetByIdAsync(assistant.TeacherId);
                if (teacher == null || teacher.IsDeleted || teacher.InstituteId != institute.Id || assistant.TeacherId != teacher.Id)
                {
                    return new AssistantCreatedResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Teacher not found or you don't have permission to delete"
                    };
                }
            }

            assistant.IsDeleted = true;
            assistant.Status = AccountStatus.Deleted;
            _unitOfWork.Assistants.Update(assistant);
            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Assistant deleted successfully" };
        }

        public async Task<AssistantProfileDTO?> GetAssistantByIdAsync(Guid assistantId, Guid userId)
        {
            var assistant = await _unitOfWork.Assistants.GetByIdAsync(assistantId);
            if (assistant == null) return null;

            var teacher = await _unitOfWork.Teachers.GetByIdAsync(userId);
            if (teacher == null || teacher.IsDeleted || teacher.Status != AccountStatus.Active || teacher.Id != assistant.TeacherId)
            {
                return null;
            }
            else
            {
                var institute = await _unitOfWork.Institutes.GetByIdAsync(userId);
                if (institute == null || institute.IsDeleted || institute.Status != AccountStatus.Active)
                {
                    return null;
                }
                teacher = await _unitOfWork.Teachers.GetByIdAsync(assistant.TeacherId);
                if (teacher == null || teacher.IsDeleted || teacher.InstituteId != institute.Id || assistant.TeacherId != teacher.Id)
                {
                    return null;
                }
            }

            return _mapper.Map<AssistantProfileDTO>(assistant);
        }

        public async Task<IEnumerable<AssistantProfileDTO>?> GetAllAssistantsAsync(Guid userId, Guid? teacherId)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(userId);
            if (teacher == null || teacher.IsDeleted || teacher.Status != AccountStatus.Active)
            {
                return null;
            }
            else if (teacherId.HasValue && teacherId.Value != userId)
            {
                var institute = await _unitOfWork.Institutes.GetByIdAsync(userId);
                if (institute == null || institute.IsDeleted || institute.Status != AccountStatus.Active)
                {
                    return null;
                }
                teacher = await _unitOfWork.Teachers.GetByIdAsync(teacherId.Value);
                if (teacher == null || teacher.IsDeleted || teacher.InstituteId != institute.Id)
                {
                    return null;
                }
            }

            var assistants = await _unitOfWork.Assistants.GetAssistantsByTeacherIdAsync(teacher.Id);
            return _mapper.Map<IEnumerable<AssistantProfileDTO>>(assistants);
        }
    }
}
