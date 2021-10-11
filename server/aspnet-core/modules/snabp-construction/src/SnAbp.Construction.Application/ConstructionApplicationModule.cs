using Microsoft.Extensions.DependencyInjection;
using SnAbp.Technology;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionDomainModule),
        typeof(ConstructionApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(TechnologyApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ConstructionApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ConstructionApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ConstructionApplicationModule>(false);
            });
        }
    }
}
