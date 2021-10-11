using Microsoft.EntityFrameworkCore;
using SnAbp.Report.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Report.EntityFrameworkCore
{
    [ConnectionStringName(ReportDbProperties.ConnectionStringName)]
    public interface IReportDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Report> Report { get; }
        DbSet<ReportRltFile> ReportAltFile { get; }
        DbSet<ReportRltUser> ReportAltUser { get; }
    }
}