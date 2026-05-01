using Omia.BLL.Repositories.Interfaces.Base;
using Omia.DAL.Models.Base;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<BaseUserEntity>
    {
        Task<bool> IsUsernameExistsAsync(string username, Guid excludeUserId);
        Task<bool> IsEmailExistsAsync(string email, Guid excludeUserId);
    }
}
