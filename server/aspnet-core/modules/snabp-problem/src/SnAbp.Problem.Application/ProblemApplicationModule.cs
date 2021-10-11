using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
//using SnAbp.Problem.Services;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(ProblemDomainModule),
        typeof(ProblemApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ProblemApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<ProblemApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<ProblemApplicationModule>();
                //options.Configurators.Add(ctx => {
                //});
            });
            
        }
    }
}
