using System;

namespace Omia.BLL.DTOs.CourseCategory
{
    public class UpdateCategoryRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AdditionalInformation { get; set; }
        public int? OrderNumber { get; set; }
    }
}
