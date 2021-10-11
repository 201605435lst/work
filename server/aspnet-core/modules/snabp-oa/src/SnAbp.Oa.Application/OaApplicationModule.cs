using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaDomainModule),
        typeof(OaApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class OaApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<OaApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<OaApplicationModule>();
            });
        }
    }
}
