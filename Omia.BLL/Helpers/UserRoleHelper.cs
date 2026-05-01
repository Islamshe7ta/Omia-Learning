using Omia.DAL.Models.Base;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Helpers
{
    public static class UserRoleHelper
    {
        public static string GetUserRole(BaseUserEntity? user)
        {
            return user switch
            {
                Admin => "Admin",
                Teacher => "Teacher",
                Student => "Student",
                Assistant => "Assistant",
                Parent => "Parent",
                Institute => "Institute",
                _ => "User"
            };
        }

        public static UserContext ResolveContext(Guid userId, string? role)
        {
            var context = new UserContext();

            if (role == "Teacher")
            {
                context.TeacherId = userId;
            }
            else if (role == "Institute")
            {
                context.InstituteId = userId;
            }

            return context;
        }
    }

    public class UserContext
    {
        public Guid? TeacherId { get; set; }
        public Guid? InstituteId { get; set; }
    }
}
