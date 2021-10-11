using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksDomainModule),
        typeof(TasksApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class TasksApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TasksApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TasksApplicationModule>(validate: false);
            });
        }
    }
}
