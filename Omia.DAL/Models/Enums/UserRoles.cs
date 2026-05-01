using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.DAL.Models.Enums
{
    public enum UserRoles
    {
        None = 0,
        Admin = 1,
        Institute = 101,
        Teacher = 201,
        Assistant = 301,
        Student = 401,
        Parent = 501,
    }
}
