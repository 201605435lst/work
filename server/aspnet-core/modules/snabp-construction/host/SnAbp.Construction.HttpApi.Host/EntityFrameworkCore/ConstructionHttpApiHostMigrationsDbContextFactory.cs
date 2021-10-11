using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Construction.EntityFrameworkCore
{
    public class ConstructionHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ConstructionHttpApiHostMigrationsDbContext>
    {
        public ConstructionHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ConstructionHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Construction"));

            return new ConstructionHttpApiHostMigrationsDbContext(builder.Options);
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
