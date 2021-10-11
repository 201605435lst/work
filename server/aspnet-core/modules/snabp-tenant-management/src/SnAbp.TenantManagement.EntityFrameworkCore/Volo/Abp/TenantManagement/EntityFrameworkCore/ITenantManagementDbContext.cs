using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.TenantManagement.EntityFrameworkCore
{
    [ConnectionStringName(SnAbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementDbContext : IEfCoreDbContext
    {
        DbSet<Tenant> Tenants { get; set; }

        DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    }
}