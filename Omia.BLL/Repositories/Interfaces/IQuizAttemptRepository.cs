using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IQuizAttemptRepository : IGenericRepository<QuizAttempt> {
        Task<QuizAttempt?> GetActiveAttemptAsync(Guid quizId, Guid studentId);
        Task<QuizAttempt?> GetAttemptWithQuizAndQuestionsAsync(Guid attemptId);
        Task<IEnumerable<QuizAttempt>> GetAttemptsByStudentAndQuizAsync(Guid quizId, Guid studentId);
        Task<IEnumerable<QuizAttempt>> GetAllAttemptsByQuizIdAsync(Guid quizId);
        Task<QuizAttempt?> GetAttemptWithAllDetailsAsync(Guid attemptId);
    }
}
