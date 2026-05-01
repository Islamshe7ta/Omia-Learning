using AutoMapper;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using Omia.PL.ViewModels.CentralAdmin;

namespace Omia.PL.AutoMapper.Profiles
{
    public class CentralAdminProfile : Profile
    {
        public CentralAdminProfile()
        {
            var today = DateTime.UtcNow;

            // Admin Mappings
            CreateMap<Admin, AdminListViewModel>();
            CreateMap<Admin, AdminFormViewModel>().ReverseMap();

            // Institute Mappings
            CreateMap<Institute, InstituteListViewModel>()
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.RegisteredStudents.Count))
                .ForMember(d => d.ActiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today)));

            CreateMap<Institute, InstituteFormViewModel>().ReverseMap();

            CreateMap<Institute, EntityStatRow>()
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.RegisteredStudents.Count))
                .ForMember(d => d.ActiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today)))
                .ForMember(d => d.InactiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate != null && c.ExpireDate <= today)))
                .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.AvailableSubjectsCount ?? 0));

            CreateMap<Institute, SelectOption>();

            // Teacher Mappings
            CreateMap<Teacher, TeacherListViewModel>()
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.RegisteredStudents.Count))
                .ForMember(d => d.ActiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today)));

            CreateMap<Teacher, TeacherFormViewModel>().ReverseMap();

            CreateMap<Teacher, EntityStatRow>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.StudentCount, o => o.MapFrom(s => s.RegisteredStudents.Count))
                .ForMember(d => d.ActiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today)))
                .ForMember(d => d.InactiveCourseCount, o => o.MapFrom(s => s.Courses.Count(c => c.ExpireDate != null && c.ExpireDate <= today)))
                .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.AvailableStagesCount ?? 0));

            CreateMap<Teacher, SelectOption>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.FullName));

            // Course Mappings
            CreateMap<Course, CourseDetailRow>()
                .ForMember(d => d.CourseId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.EnrolledStudents, o => o.MapFrom(s => s.CourseStudents.Count))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.ExpireDate == null || s.ExpireDate > today));

            CreateMap<Course, SelectOption>();

            // Dashboard Mapping
            CreateMap<(List<Institute> Institutes, List<Teacher> SoloTeachers), DashboardViewModel>()
                .ForMember(d => d.TotalInstitutes, o => o.MapFrom(s => s.Institutes.Count))
                .ForMember(d => d.ActiveInstitutes, o => o.MapFrom(s => s.Institutes.Count(i => i.Status == AccountStatus.Active)))
                .ForMember(d => d.TotalSoloTeachers, o => o.MapFrom(s => s.SoloTeachers.Count))
                .ForMember(d => d.ActiveSoloTeachers, o => o.MapFrom(s => s.SoloTeachers.Count(t => t.Status == AccountStatus.Active)))
                .ForMember(d => d.TotalStudentsInInstitutes, o => o.MapFrom(s => s.Institutes.Sum(i => i.RegisteredStudents.Count)))
                .ForMember(d => d.TotalStudentsWithSoloTeachers, o => o.MapFrom(s => s.SoloTeachers.Sum(t => t.RegisteredStudents.Count)))
                .ForMember(d => d.ActiveCourses, o => o.MapFrom(s => s.Institutes.Sum(i => i.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today))
                                                              + s.SoloTeachers.Sum(t => t.Courses.Count(c => c.ExpireDate == null || c.ExpireDate > today))))
                .ForMember(d => d.InactiveCourses, o => o.MapFrom(s => s.Institutes.Sum(i => i.Courses.Count(c => c.ExpireDate != null && c.ExpireDate <= today))
                                                                + s.SoloTeachers.Sum(t => t.Courses.Count(c => c.ExpireDate != null && c.ExpireDate <= today))))
                .ForMember(d => d.TotalAvailableSubjectsForInstitutes, o => o.MapFrom(s => s.Institutes.Sum(i => i.AvailableSubjectsCount ?? 0)))
                .ForMember(d => d.TotalAvailableStagesForTeachers, o => o.MapFrom(s => s.SoloTeachers.Sum(t => t.AvailableStagesCount ?? 0)))
                .ForMember(d => d.InstituteStats, o => o.MapFrom(s => s.Institutes))
                .ForMember(d => d.SoloTeacherStats, o => o.MapFrom(s => s.SoloTeachers));

            // Details Mappings
            CreateMap<DetailsFilterViewModel, DetailsViewModel>()
                .ForMember(d => d.Filter, o => o.MapFrom(s => s));
        }
    }
}
