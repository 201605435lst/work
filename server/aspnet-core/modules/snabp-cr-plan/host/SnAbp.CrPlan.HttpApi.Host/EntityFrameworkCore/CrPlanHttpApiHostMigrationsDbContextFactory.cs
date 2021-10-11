using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.CrPlan.EntityFrameworkCore
{
    public class CrPlanHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CrPlanHttpApiHostMigrationsDbContext>
    {
        public CrPlanHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CrPlanHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("CrPlan"));
                .UseNpgsql(configuration.GetConnectionString("CrPlan"));

            return new CrPlanHttpApiHostMigrationsDbContext(builder.Options);
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
