using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.FileApprove.EntityFrameworkCore
{
    public class FileApproveHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<FileApproveHttpApiHostMigrationsDbContext>
    {
        public FileApproveHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<FileApproveHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("FileApprove"));

            return new FileApproveHttpApiHostMigrationsDbContext(builder.Options);
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
