using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IQuizQuestionRepository : IGenericRepository<QuizQuestion> {
        Task DeleteQuestionsByQuizIdAsync(Guid quizId);
    }
}
