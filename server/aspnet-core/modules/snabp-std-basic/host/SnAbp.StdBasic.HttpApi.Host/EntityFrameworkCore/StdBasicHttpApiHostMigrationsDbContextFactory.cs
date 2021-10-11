using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.StdBasic.EntityFrameworkCore
{
    public class StdBasicHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<StdBasicHttpApiHostMigrationsDbContext>
    {
        public StdBasicHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<StdBasicHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("StdBasic"));
                .UseNpgsql(configuration.GetConnectionString("StdBasic"));

            return new StdBasicHttpApiHostMigrationsDbContext(builder.Options);
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
