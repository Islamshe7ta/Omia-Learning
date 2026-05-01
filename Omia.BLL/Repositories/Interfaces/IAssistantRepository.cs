using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Interfaces.Base;

namespace Omia.BLL.Repositories.Interfaces
{
    public interface IAssistantRepository : IGenericRepository<Assistant> 
    {
        Task<Assistant?> GetByIdAndTeacherIdAsync(Guid id, Guid teacherId);
        Task<IEnumerable<Assistant>> GetAssistantsByTeacherIdAsync(Guid teacherId);
    }
}
