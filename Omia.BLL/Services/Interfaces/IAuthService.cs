using Omia.BLL.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Interfaces
{
    public interface IAuthService
    {
       public Task<LoginResponseDTO> Login(string username, string password);
    }
}
