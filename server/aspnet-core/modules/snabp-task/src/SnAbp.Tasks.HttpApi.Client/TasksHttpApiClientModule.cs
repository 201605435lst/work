using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Tasks
{
    [DependsOn(
        typeof(TasksApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class TasksHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Tasks";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TasksApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
