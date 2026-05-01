using Omia.BLL.DTOs;

namespace Omia.BLL.DTOs.CourseContent
{
    public class CourseContentDetailsDTO : BaseResponseDTO
    {
        public ContentDetailsDTO? Content { get; set; }
    }
}