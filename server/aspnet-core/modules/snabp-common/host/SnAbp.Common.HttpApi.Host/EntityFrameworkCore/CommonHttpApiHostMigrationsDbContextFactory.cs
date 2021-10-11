using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Common.EntityFrameworkCore
{
    public class CommonHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<CommonHttpApiHostMigrationsDbContext>
    {
        public CommonHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CommonHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Common"));
                .UseNpgsql(configuration.GetConnectionString("Common"));

            return new CommonHttpApiHostMigrationsDbContext(builder.Options);
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
