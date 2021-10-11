using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Resource.EntityFrameworkCore
{
    public class ResourceHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ResourceHttpApiHostMigrationsDbContext>
    {
        public ResourceHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ResourceHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Resource"));
                .UseNpgsql(configuration.GetConnectionString("Resource"));

            return new ResourceHttpApiHostMigrationsDbContext(builder.Options);
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
