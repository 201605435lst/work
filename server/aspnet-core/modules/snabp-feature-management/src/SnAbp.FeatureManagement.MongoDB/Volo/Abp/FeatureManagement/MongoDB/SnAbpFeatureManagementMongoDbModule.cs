using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace SnAbp.FeatureManagement.MongoDB
{
    [DependsOn(
        typeof(SnAbpFeatureManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class SnAbpFeatureManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<FeatureManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<IFeatureManagementMongoDbContext>();

                options.AddRepository<FeatureValue, MongoFeatureValueRepository>();
            });
        }
    }
}
