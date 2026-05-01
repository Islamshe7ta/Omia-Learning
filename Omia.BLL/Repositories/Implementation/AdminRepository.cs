using Microsoft.EntityFrameworkCore;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;

namespace Omia.BLL.Repositories.Implementation
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(OmiaDbContext context) : base(context) { }

        public async Task<Admin?> GetByUsernameAsync(string username)
            => await _context.Admins.FirstOrDefaultAsync(a => a.Username == username && !a.IsDeleted);

        public async Task<bool> IsUsernameUsedAsync(string username, Guid? excludeId = null)
        {
            var match = await _context.Admins.FirstOrDefaultAsync(
                a => a.Username == username && !a.IsDeleted && (excludeId == null || a.Id != excludeId));
            return match != null;
        }
    }
}
