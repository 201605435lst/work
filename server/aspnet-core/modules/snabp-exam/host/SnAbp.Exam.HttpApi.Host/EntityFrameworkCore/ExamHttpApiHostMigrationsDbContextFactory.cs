using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SnAbp.Exam.EntityFrameworkCore
{
    public class ExamHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ExamHttpApiHostMigrationsDbContext>
    {
        public ExamHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ExamHttpApiHostMigrationsDbContext>()
                //.UseSqlServer(configuration.GetConnectionString("Exam"));
                .UseNpgsql(configuration.GetConnectionString("Exam"));

            return new ExamHttpApiHostMigrationsDbContext(builder.Options);
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
