using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using SnAbp.Message.Notice;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeDomainModule),
        typeof(SafeApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(SnAbpMessageNoticeModule),
        typeof(AbpAutoMapperModule)
        )]
    public class SafeApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SafeApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SafeApplicationModule>();
            });
        }
    }
}
