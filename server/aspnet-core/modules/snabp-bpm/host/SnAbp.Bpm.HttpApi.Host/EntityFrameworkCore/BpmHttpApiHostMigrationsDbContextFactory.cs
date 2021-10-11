using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    public class BpmHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<BpmHttpApiHostMigrationsDbContext>
    {
        public BpmHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BpmHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Bpm"));
                .UseNpgsql(configuration.GetConnectionString("Bpm"));

            return new BpmHttpApiHostMigrationsDbContext(builder.Options);
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
