using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;

namespace SnAbp.BackgroundJobs.EntityFrameworkCore
{
    [ConnectionStringName(BackgroundJobsDbProperties.ConnectionStringName)]
    public class BackgroundJobsDbContext : AbpDbContext<BackgroundJobsDbContext>, IBackgroundJobsDbContext
    {
        public DbSet<BackgroundJobRecord> BackgroundJobs { get; set; }

        public BackgroundJobsDbContext(DbContextOptions<BackgroundJobsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs();
        }
    }
}