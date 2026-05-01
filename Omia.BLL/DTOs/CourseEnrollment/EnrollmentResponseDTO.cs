using System;
using Omia.BLL.DTOs;

namespace Omia.BLL.DTOs.CourseEnrollment
{
    public class EnrollmentResponseDTO : BaseResponseDTO
    {
        public Guid EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
