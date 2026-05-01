using System;
using Omia.BLL.DTOs;

namespace Omia.BLL.DTOs.Teacher
{
    public class TeacherCreatedResponseDTO : BaseResponseDTO
    {
        public Guid TeacherId { get; set; }
    }
}
