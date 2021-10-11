using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.ConstructionBase.EntityFrameworkCore
{
    public class ConstructionBaseHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ConstructionBaseHttpApiHostMigrationsDbContext>
    {
        public ConstructionBaseHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ConstructionBaseHttpApiHostMigrationsDbContext>()
                .UseNpgsql(configuration.GetConnectionString("ConstructionBase"));

            return new ConstructionBaseHttpApiHostMigrationsDbContext(builder.Options);
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
