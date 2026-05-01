namespace Omia.PL.ViewModels.CentralAdmin
{
    public class DashboardViewModel
    {
        // Subscription Stats
        public int TotalInstitutes { get; set; }
        public int ActiveInstitutes { get; set; }
        public int TotalSoloTeachers { get; set; }
        public int ActiveSoloTeachers { get; set; }

        // Student Stats
        public int TotalStudentsInInstitutes { get; set; }
        public int TotalStudentsWithSoloTeachers { get; set; }
        public int TotalStudents => TotalStudentsInInstitutes + TotalStudentsWithSoloTeachers;

        // Course Stats
        public int ActiveCourses { get; set; }       // ExpireDate > today OR ExpireDate == null
        public int InactiveCourses { get; set; }     // ExpireDate <= today

        // Available Slots
        public int TotalAvailableSubjectsForInstitutes { get; set; }   // sum of AvailableSubjectsCount
        public int TotalAvailableStagesForTeachers { get; set; }       // sum of AvailableStagesCount

        // Per-entity lists for quick stats tables
        public List<EntityStatRow> InstituteStats { get; set; } = new();
        public List<EntityStatRow> SoloTeacherStats { get; set; } = new();
    }

    public class EntityStatRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public int ActiveCourseCount { get; set; }
        public int InactiveCourseCount { get; set; }
        public int AvailableSlots { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
