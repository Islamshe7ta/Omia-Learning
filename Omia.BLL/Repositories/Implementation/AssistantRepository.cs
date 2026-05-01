using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omia.BLL.Repositories.Implementation
{
    public class AssistantRepository : GenericRepository<Assistant>, IAssistantRepository
    {
        public AssistantRepository(OmiaDbContext context) : base(context) { }

        public async Task<Assistant?> GetByIdAndTeacherIdAsync(Guid id, Guid teacherId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.TeacherId == teacherId);
        }

        public async Task<IEnumerable<Assistant>> GetAssistantsByTeacherIdAsync(Guid teacherId)
        {
            return await _dbSet.Where(x => x.TeacherId == teacherId && !x.IsDeleted).ToListAsync();
        }
    }
}
