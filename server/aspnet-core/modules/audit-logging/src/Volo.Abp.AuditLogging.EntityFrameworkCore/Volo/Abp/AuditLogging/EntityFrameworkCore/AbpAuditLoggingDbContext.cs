using Microsoft.EntityFrameworkCore;

using SnAbp.EntityFrameworkCore;

using Volo.Abp.Data;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AbpAuditLoggingDbProperties.ConnectionStringName)]
    public class AbpAuditLoggingDbContext : AbpDbContext<AbpAuditLoggingDbContext>, IAuditLoggingDbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AbpAuditLoggingDbContext(DbContextOptions<AbpAuditLoggingDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();
        }
    }
}
