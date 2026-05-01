using AutoMapper;
using Omia.BLL.DTOs.Assignment;
using Omia.BLL.DTOs.StudentAssignment;
using Omia.BLL.DTOs.Student;
using Omia.BLL.DTOs.TeacherAssignment;
using Omia.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateAssignmentDTO, Assignment>();
            CreateMap<UpdateAssignmentDTO, Assignment>();
            CreateMap<Assignment, AssignmentsCourseResponse>();
            CreateMap<SubmitAssignmentDTO, AssignmentSubmission>();
            CreateMap<AssignmentSubmission, MySubmissionDTO>();
            CreateMap<CorrectAssignmentSubmissionDTO, AssignmentSubmission>();
            CreateMap<Omia.DAL.Models.Entities.Student, StudentBriefDTO>();
            CreateMap<AssignmentSubmission, AssignmentSubmissionTeacherDTO>();
        }
    }
}
