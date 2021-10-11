using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.Common.Services;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(CommonDomainModule),
        typeof(CommonApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class CommonApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CommonApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CommonApplicationModule>(false);
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
