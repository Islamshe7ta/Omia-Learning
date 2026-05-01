using System;

namespace Omia.DAL.Models.Enums
{
    [Flags]
    public enum AdminPermissions
    {
        None = 0,
        AddAdmin = 1,
        AddInstitute = 2,
        AddIndependentTeacher = 4
    }
}
