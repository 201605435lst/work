using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    public class AlarmHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AlarmHttpApiHostMigrationsDbContext>
    {
        public AlarmHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AlarmHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Alarm"));

            return new AlarmHttpApiHostMigrationsDbContext(builder.Options);
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
