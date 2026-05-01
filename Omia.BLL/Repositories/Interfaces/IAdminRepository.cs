using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null);
    }
}
