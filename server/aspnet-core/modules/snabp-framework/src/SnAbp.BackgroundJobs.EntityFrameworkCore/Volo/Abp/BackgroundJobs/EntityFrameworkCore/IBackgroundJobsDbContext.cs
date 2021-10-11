using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;

namespace SnAbp.BackgroundJobs.EntityFrameworkCore
{
    [ConnectionStringName(BackgroundJobsDbProperties.ConnectionStringName)]
    public interface IBackgroundJobsDbContext : IEfCoreDbContext
    {
        DbSet<BackgroundJobRecord> BackgroundJobs { get; }
    }
}