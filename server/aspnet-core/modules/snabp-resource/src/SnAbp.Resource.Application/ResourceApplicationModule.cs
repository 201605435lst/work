using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Caching;
//using SnAbp.Resource.Services;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(ResourceDomainModule),
        typeof(ResourceApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpCachingModule)
        )]
    public class ResourceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ResourceApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ResourceApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
