using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.TenantManagement.MongoDB
{
    [ConnectionStringName(SnAbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Tenant> Tenants { get; }
    }
}