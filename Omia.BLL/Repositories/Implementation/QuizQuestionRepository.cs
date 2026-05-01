using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Omia.BLL.Repositories.Implementation
{
    public class QuizQuestionRepository : GenericRepository<QuizQuestion>, IQuizQuestionRepository
    {
        public QuizQuestionRepository(OmiaDbContext context) : base(context) { }

        public async Task DeleteQuestionsByQuizIdAsync(Guid quizId)
        {
            await _dbSet.Where(q => q.QuizId == quizId && !q.IsDeleted)
                        .ExecuteUpdateAsync(s => s
                            .SetProperty(p => p.IsDeleted, true)
                            .SetProperty(p => p.LastUpdatedAt, DateTime.UtcNow));
        }
    }
}
