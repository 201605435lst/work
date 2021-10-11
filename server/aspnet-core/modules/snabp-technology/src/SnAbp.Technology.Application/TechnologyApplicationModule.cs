using Microsoft.Extensions.DependencyInjection;
using SnAbp.Technology.IServices;
using SnAbp.Technology.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Caching;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyDomainModule),
        typeof(TechnologyApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpCachingModule)
        )]
    public class TechnologyApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TechnologyApplicationModule>();
            context.Services.AddTransient<IQuantityManagerAppService, TechnologyQuantityAppService>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TechnologyApplicationModule>();
            });
        }
    }
}
