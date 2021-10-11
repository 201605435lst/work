using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace SnAbp.SettingManagement.MongoDB
{
    [DependsOn(
        typeof(SnAbpSettingManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class SnAbpSettingManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<SettingManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ISettingManagementMongoDbContext>();

                options.AddRepository<Setting, MongoSettingRepository>();
            });
        }
    }
}
