using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IQuizRepository : IGenericRepository<Quiz>
    {
        Task<Quiz?> GetQuizWithQuestionsAsync(Guid quizId);
        Task<IEnumerable<Quiz>> GetQuizzesByCourseIdAsync(Guid courseId);
    }
}
