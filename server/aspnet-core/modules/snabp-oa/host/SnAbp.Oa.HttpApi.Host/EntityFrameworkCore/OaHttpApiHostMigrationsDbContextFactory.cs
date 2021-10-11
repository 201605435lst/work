using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Oa.EntityFrameworkCore
{
    public class OaHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<OaHttpApiHostMigrationsDbContext>
    {
        public OaHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OaHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Oa"));

            return new OaHttpApiHostMigrationsDbContext(builder.Options);
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
