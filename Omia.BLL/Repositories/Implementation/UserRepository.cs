using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Base;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.Repositories.Implementation
{
    public class UserRepository : GenericRepository<BaseUserEntity>, IUserRepository
    {
        public UserRepository(OmiaDbContext context) : base(context) { }

        public async Task<bool> IsUsernameExistsAsync(string username, Guid excludeUserId)
        {
            return await _dbSet.AnyAsync(u => u.Username == username && u.Id != excludeUserId && !u.IsDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, Guid excludeUserId)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.Id != excludeUserId && !u.IsDeleted);
        }
    }
}
