using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Report.EntityFrameworkCore
{
    public class ReportHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ReportHttpApiHostMigrationsDbContext>
    {
        public ReportHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ReportHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Report"));

            return new ReportHttpApiHostMigrationsDbContext(builder.Options);
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
