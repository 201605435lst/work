using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    public class ScheduleHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ScheduleHttpApiHostMigrationsDbContext>
    {
        public ScheduleHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ScheduleHttpApiHostMigrationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("Schedule"));

            return new ScheduleHttpApiHostMigrationsDbContext(builder.Options);
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
