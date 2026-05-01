using Omia.BLL.DTOs;
using Omia.BLL.DTOs.CourseContent;

namespace Omia.BLL.Services.Interfaces
{
    public interface ICourseContentService
    {
        Task<CreateCourseContentResponseDTO> CreateCourseContentAsync(CreateCourseContentDTO createCourseContentDTO, Guid uploaderId);
        Task<BaseResponseDTO> UpdateCourseContentAsync(UpdateCourseContentDTO updateCourseContentDTO, Guid courseContentId, Guid userId);
        Task<BaseResponseDTO> DeleteCourseContentAsync(Guid courseContentId, Guid userId);
        Task<CourseContentDetailsDTO> GetCourseContentDetailsAsync(Guid contentId, Guid userId);
        Task<CommentResponseDTO> AddCommentAsync(AddContentCommentDTO request, Guid userId);
        Task<ContentCountDTO> GetCourseContentCountAsync(Guid courseId, Guid userId);
        Task<IEnumerable<VideoSummaryDTO>?> GetNewestVideosAsync(Guid courseId, Guid userId);
        Task<IEnumerable<ContentDetailsDTO>> GetCourseContentsAsync(Guid courseId, Guid userId);
        Task<IEnumerable<ContentDetailsDTO>> GetCategoryContentsAsync(Guid categoryId, Guid userId);

    }
}
