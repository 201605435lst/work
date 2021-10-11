using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Bpm;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(SnAbpMessageBpmModule),
        typeof(BpmDomainModule),
        typeof(BpmApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class BpmApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BpmApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BpmApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnApplicationInitialization(context);
        }
    }
}
