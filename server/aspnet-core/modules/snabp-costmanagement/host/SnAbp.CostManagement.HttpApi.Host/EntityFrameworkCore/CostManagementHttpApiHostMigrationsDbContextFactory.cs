using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.CostManagement.EntityFrameworkCore
{
    public class CostManagementHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CostManagementHttpApiHostMigrationsDbContext>
    {
        public CostManagementHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CostManagementHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("CostManagement"));

            return new CostManagementHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
