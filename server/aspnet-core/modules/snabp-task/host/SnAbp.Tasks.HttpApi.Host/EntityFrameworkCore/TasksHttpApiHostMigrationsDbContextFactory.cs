using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    public class TasksHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<TasksHttpApiHostMigrationsDbContext>
    {
        public TasksHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<TasksHttpApiHostMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Tasks"));

            return new TasksHttpApiHostMigrationsDbContext(builder.Options);
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
