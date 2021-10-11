using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.TenantManagement.EntityFrameworkCore
{
    [ConnectionStringName(SnAbpTenantManagementDbProperties.ConnectionStringName)]
    public class TenantManagementDbContext : AbpDbContext<TenantManagementDbContext>, ITenantManagementDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTenantManagement();
        }
    }
}
