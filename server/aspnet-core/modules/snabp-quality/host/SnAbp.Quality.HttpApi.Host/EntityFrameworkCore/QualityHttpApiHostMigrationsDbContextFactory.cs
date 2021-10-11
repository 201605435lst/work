using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Quality.EntityFrameworkCore
{
    public class QualityHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<QualityHttpApiHostMigrationsDbContext>
    {
        public QualityHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<QualityHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Quality"));

            return new QualityHttpApiHostMigrationsDbContext(builder.Options);
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
