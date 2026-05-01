using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ILiveSessionRepository : IGenericRepository<LiveSession>
    {
        Task<IEnumerable<LiveSession>> GetLiveSessionsByCourseId(Guid courseId);

    
    }
}
