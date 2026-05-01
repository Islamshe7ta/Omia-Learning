using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        private readonly OmiaDbContext _context;
        public QuizRepository(OmiaDbContext context) : base(context)
        {

            _context = context;
        }

        public async Task<Quiz?> GetQuizWithQuestionsAsync(Guid quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync(q => q.Id == quizId && !q.IsDeleted);
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByCourseIdAsync(Guid courseId)
        {
            return await _context.Quizzes
                .Where(q => q.CourseId == courseId && !q.IsDeleted)
                .OrderBy(q => q.OrderNumber)
                .ToListAsync();
        }
    }
}
