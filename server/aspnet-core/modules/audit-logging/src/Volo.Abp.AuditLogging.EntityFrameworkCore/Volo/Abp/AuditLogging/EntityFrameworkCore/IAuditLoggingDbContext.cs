using Microsoft.EntityFrameworkCore;

using SnAbp.EntityFrameworkCore;

using Volo.Abp.Data;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AbpAuditLoggingDbProperties.ConnectionStringName)]
    public interface IAuditLoggingDbContext : IEfCoreDbContext
    {
        DbSet<AuditLog> AuditLogs { get; set; }
    }
}