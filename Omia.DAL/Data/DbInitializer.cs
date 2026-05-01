using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System.Linq;

namespace Omia.DAL.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<OmiaDbContext>();

                    // 1. Run Migrations
                    context.Database.Migrate();

                    // 2. Seed Default Admin
                    if (!context.Admins.Any())
                    {
                        var admin = new Admin
                        {
                            FullName = "System Admin",
                            Username = "admin",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                            Email = "admin@omia.com",
                            Status = AccountStatus.Active
                        };

                        context.Admins.Add(admin);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while Initialize the database: \n" + ex.Message);
            }
        }
    }
}
