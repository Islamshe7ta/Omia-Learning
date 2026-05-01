using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Omia.DAL.Data
{
    public class OmiaDbContextFactory : IDesignTimeDbContextFactory<OmiaDbContext>
    {
        public OmiaDbContext CreateDbContext(string[] args)
        {
            // Build configuration to get connection string from appsettings.json in the PL project
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Omia.PL"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OmiaDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseSqlServer(connectionString);

            return new OmiaDbContext(optionsBuilder.Options);
        }

    }
}
