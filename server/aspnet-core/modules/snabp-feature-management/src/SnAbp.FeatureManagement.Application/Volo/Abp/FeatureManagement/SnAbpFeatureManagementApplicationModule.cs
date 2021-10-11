using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementDomainModule),
        typeof(SnAbpFeatureManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class SnAbpFeatureManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SnAbpFeatureManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<FeatureManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
