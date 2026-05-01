using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Repositories.Implementation
{
    public class LiveSessionRepository : GenericRepository<LiveSession>, ILiveSessionRepository
    {
        private readonly OmiaDbContext _context;


        public LiveSessionRepository(OmiaDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<LiveSession>> GetLiveSessionsByCourseId(Guid courseId)
        {
            var sessions = await _context.Set<LiveSession>()
                                         .Where(ls => ls.CourseId == courseId)
                                         .OrderBy(ls => ls.OrderNumber) 
                                         .ToListAsync();

            return sessions;
        }
    }
}
