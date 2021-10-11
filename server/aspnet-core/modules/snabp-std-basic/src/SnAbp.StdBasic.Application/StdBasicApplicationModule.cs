using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.StdBasic.Services;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicDomainModule),
        typeof(StdBasicApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class StdBasicApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<StdBasicApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<StdBasicApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
