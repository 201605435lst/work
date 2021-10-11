using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    public class ComponentTrackHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ComponentTrackHttpApiHostMigrationsDbContext>
    {
        public ComponentTrackHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ComponentTrackHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("ComponentTrack"));

            return new ComponentTrackHttpApiHostMigrationsDbContext(builder.Options);
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
