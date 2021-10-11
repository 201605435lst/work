using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.TenantManagement.MongoDB
{
    [ConnectionStringName(SnAbpTenantManagementDbProperties.ConnectionStringName)]
    public class TenantManagementMongoDbContext : AbpMongoDbContext, ITenantManagementMongoDbContext
    {
        public IMongoCollection<Tenant> Tenants => Collection<Tenant>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureTenantManagement();
        }
    }
}