using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.Emerg.Services;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(EmergDomainModule),
        typeof(EmergApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class EmergApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<EmergApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<EmergApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
