using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace SnAbp.TenantManagement.MongoDB
{
    [DependsOn(
        typeof(SnAbpTenantManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class SnAbpTenantManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<TenantManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementMongoDbContext>();

                options.AddRepository<Tenant, MongoTenantRepository>();
            });
        }
    }
}
