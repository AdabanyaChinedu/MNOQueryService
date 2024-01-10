using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MNOQueryService.Persistence.DatabaseContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MNODbContext>
    {
        /// <summary>
        /// gets the TenantTeamMember property.
        /// </summary>
        /// <returns>TenantServiceDBContext.</returns>
        /// <param name="args">args.</param>
        public MNODbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<MNODbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlite(connectionString!);
            return new MNODbContext(builder.Options);
        }
    }
}
