using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Problem.EntityFrameworkCore
{
    public class ProblemHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ProblemHttpApiHostMigrationsDbContext>
    {
        public ProblemHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ProblemHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Problem"));
                .UseNpgsql(configuration.GetConnectionString("Problem"));

            return new ProblemHttpApiHostMigrationsDbContext(builder.Options);
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
