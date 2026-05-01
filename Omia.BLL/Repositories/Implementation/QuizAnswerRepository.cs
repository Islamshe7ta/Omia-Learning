using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;

namespace Omia.BLL.Repositories.Implementation
{
    public class QuizAnswerRepository : GenericRepository<QuizAnswer>, IQuizAnswerRepository
    {
        public QuizAnswerRepository(OmiaDbContext context) : base(context) { }
    }
}
