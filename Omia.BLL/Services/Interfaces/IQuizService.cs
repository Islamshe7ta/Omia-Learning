using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Assignment;
using Omia.BLL.DTOs.Quiz;
using Omia.BLL.DTOs.QuizAttempt;

namespace Omia.BLL.Services.Interfaces
{
    public interface IQuizService
    {

        Task<QuizResponseDTO> CreateQuizAsync(CreateQuizDTO createQuizDTO, Guid userId);
        Task<BaseResponseDTO> UpdateQuizAsync(Guid QuizId, UpdateQuizDTO updateQuizDTO, Guid userId);
        Task<BaseResponseDTO> DeleteQuizAsync(Guid quizId, Guid userId);
        Task<QuizDetailsResponseDTO> GetQuizDetailsAsync(Guid quizId, Guid userId);
        Task<IEnumerable<QuizzesCourseResponse>> GetQuizzesByCourseIdAsync(Guid courseId, Guid userId);
        Task<StartQuizResponseDTO> StartQuizAsync(StartQuizRequestDTO request, Guid studentId);
        Task<EndQuizResponseDTO> EndQuizAsync(EndQuizRequestDTO request, Guid studentId);
        Task<IEnumerable<MyQuizAttemptDTO>> GetMyAttemptsAsync(Guid quizId, Guid studentId);
        Task<GetQuizAttemptsForTeacherResponseDTO> GetQuizAttemptsForTeacherAsync(Guid quizId, Guid userId);
        Task<QuizAttemptDetailsResponseDTO> GetQuizAttemptDetailsAsync(Guid attemptId, Guid userId);
    }
}
