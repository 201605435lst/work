using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace SnAbp.PermissionManagement.MongoDB
{
    [DependsOn(
        typeof(SnAbpPermissionManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpPermissionManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<PermissionManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementMongoDbContext>();

                options.AddRepository<PermissionGrant, MongoPermissionGrantRepository>();
            });
        }
    }
}
