using AutoMapper;
using Omia.BLL.DTOs.CourseEnrollment;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class CourseEnrollmentProfile : Profile
    {
        public CourseEnrollmentProfile()
        {
            CreateMap<AssignStudentToCourseDTO, CourseStudent>();
        }
    }
}
