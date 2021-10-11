using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Technology.EntityFrameworkCore
{
    public class TechnologyHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<TechnologyHttpApiHostMigrationsDbContext>
    {
        public TechnologyHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<TechnologyHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Technology"));

            return new TechnologyHttpApiHostMigrationsDbContext(builder.Options);
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
