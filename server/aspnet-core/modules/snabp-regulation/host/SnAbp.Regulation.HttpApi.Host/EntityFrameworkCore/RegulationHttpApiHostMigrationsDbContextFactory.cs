using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Regulation.EntityFrameworkCore
{
    public class RegulationHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<RegulationHttpApiHostMigrationsDbContext>
    {
        public RegulationHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<RegulationHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Regulation"));

            return new RegulationHttpApiHostMigrationsDbContext(builder.Options);
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
