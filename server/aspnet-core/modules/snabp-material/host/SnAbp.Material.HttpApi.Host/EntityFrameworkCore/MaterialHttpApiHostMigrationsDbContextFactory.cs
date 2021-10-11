using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Material.EntityFrameworkCore
{
    public class MaterialHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<MaterialHttpApiHostMigrationsDbContext>
    {
        public MaterialHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<MaterialHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Material"));

            return new MaterialHttpApiHostMigrationsDbContext(builder.Options);
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
