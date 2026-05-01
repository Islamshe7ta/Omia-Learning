using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<IEnumerable<Teacher>> GetSoloTeachersWithDetailsAsync();
        Task<Teacher?> GetWithDetailsByIdAsync(Guid id);
        Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null);
        Task<Teacher?> GetByIdAndInstituteIdAsync(Guid id, Guid? instituteId);
        Task<IEnumerable<Teacher>> GetAllTeachersByContextAsync(Guid? instituteId);
    }
}
