using Microsoft.EntityFrameworkCore;
using SnAbp.Report.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Report.EntityFrameworkCore
{
    [ConnectionStringName(ReportDbProperties.ConnectionStringName)]
    public class ReportDbContext : AbpDbContext<ReportDbContext>, IReportDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public ReportDbContext(DbContextOptions<ReportDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Report> Report { get; set; }

        public DbSet<ReportRltFile> ReportAltFile { get; set; }

        public DbSet<ReportRltUser> ReportAltUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureReport();
        }
    }
}