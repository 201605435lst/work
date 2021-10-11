using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Report.EntityFrameworkCore
{
    public class ReportHttpApiHostMigrationsDbContext : AbpDbContext<ReportHttpApiHostMigrationsDbContext>
    {
        public ReportHttpApiHostMigrationsDbContext(DbContextOptions<ReportHttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureReport();
        }
    }
}
