namespace Omia.PL.ViewModels.CentralAdmin
{
    public class DetailsFilterViewModel
    {
        public Guid? InstituteId { get; set; }
        public Guid? TeacherId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? CourseId { get; set; }
    }

    public class DetailsViewModel
    {
        public DetailsFilterViewModel Filter { get; set; } = new();

        // Dropdown options
        public List<SelectOption> Institutes { get; set; } = new();
        public List<SelectOption> SoloTeachers { get; set; } = new();
        public List<SelectOption> Courses { get; set; } = new();

        // Results
        public string? SelectedEntityName { get; set; }
        public int StudentCount { get; set; }
        public List<CourseDetailRow> CourseRows { get; set; } = new();
    }

    public class CourseDetailRow
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int EnrolledStudents { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpireDate { get; set; }
    }

    public class SelectOption
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
