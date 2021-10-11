using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Safe.EntityFrameworkCore
{
    public class SafeHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<SafeHttpApiHostMigrationsDbContext>
    {
        public SafeHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<SafeHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Safe"));

            return new SafeHttpApiHostMigrationsDbContext(builder.Options);
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
