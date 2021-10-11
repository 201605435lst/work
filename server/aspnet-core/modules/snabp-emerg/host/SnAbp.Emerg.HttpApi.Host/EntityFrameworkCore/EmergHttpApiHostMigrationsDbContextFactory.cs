using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    public class EmergHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<EmergHttpApiHostMigrationsDbContext>
    {
        public EmergHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<EmergHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Emerg"));
                .UseNpgsql(configuration.GetConnectionString("Emerg"));

            return new EmergHttpApiHostMigrationsDbContext(builder.Options);
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
