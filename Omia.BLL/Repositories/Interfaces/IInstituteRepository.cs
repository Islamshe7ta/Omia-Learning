using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IInstituteRepository : IGenericRepository<Institute>
    {
        Task<IEnumerable<Institute>> GetAllWithDetailsAsync();
        Task<Institute?> GetWithDetailsByIdAsync(Guid id);
        Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null);
    }
}
