using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Guids;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationDomainModule),
        typeof(RegulationApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class RegulationApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<RegulationApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<RegulationApplicationModule>(validate: false);
            });
        }
    }
}
