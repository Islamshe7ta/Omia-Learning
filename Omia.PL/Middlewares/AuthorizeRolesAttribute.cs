using Omia.DAL.Models.Enums;

namespace Omia.PL.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeRolesAttribute : Attribute
    {
        public UserRoles[] Roles { get; }

        public AuthorizeRolesAttribute(params UserRoles[] roles)
        {
            Roles = roles;
        }
    }
}
