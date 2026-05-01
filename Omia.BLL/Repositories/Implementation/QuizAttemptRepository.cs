using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Repositories.Implementation
{
    public class QuizAttemptRepository : GenericRepository<QuizAttempt>, IQuizAttemptRepository
    {
        public QuizAttemptRepository(OmiaDbContext context) : base(context) { }

        public async Task<QuizAttempt?> GetActiveAttemptAsync(Guid quizId, Guid studentId)
        {
            return await FirstOrDefaultAsync(x => x.QuizId == quizId && 
                                                 x.StudentId == studentId && 
                                                 x.Status == QuizAttemptStatus.InProgress);
        }

        public async Task<QuizAttempt?> GetAttemptWithQuizAndQuestionsAsync(Guid attemptId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync(a => a.Id == attemptId && !a.IsDeleted);
        }
 
        public async Task<IEnumerable<QuizAttempt>> GetAttemptsByStudentAndQuizAsync(Guid quizId, Guid studentId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions.Where(x => !x.IsDeleted))
                .Include(a => a.Answers.Where(x => !x.IsDeleted))
                .Where(a => a.QuizId == quizId && a.StudentId == studentId && !a.IsDeleted)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }
 
        public async Task<IEnumerable<QuizAttempt>> GetAllAttemptsByQuizIdAsync(Guid quizId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Student)
                .Where(a => a.QuizId == quizId && !a.IsDeleted)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }
        public async Task<QuizAttempt?> GetAttemptWithAllDetailsAsync(Guid attemptId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Quiz)
                .Include(a => a.Answers.Where(x => !x.IsDeleted))
                    .ThenInclude(ans => ans.Question)
                .FirstOrDefaultAsync(a => a.Id == attemptId && !a.IsDeleted);
        }
    }
}
