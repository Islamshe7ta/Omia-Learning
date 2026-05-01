namespace Omia.BLL.DTOs.CentralAdmin
{
    public class DetailsResultDto
    {
        public string SelectedEntityName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public List<CourseDetailDto> Courses { get; set; } = [];
    }
}
