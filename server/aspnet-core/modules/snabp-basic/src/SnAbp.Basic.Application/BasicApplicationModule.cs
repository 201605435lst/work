using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.Basic.Services;

namespace SnAbp.Basic
{
    [DependsOn(
        typeof(BasicDomainModule),
        typeof(BasicApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class BasicApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BasicApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BasicApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
