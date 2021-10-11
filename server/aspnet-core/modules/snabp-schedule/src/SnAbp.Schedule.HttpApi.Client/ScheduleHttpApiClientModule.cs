using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Schedule
{
    [DependsOn(
        typeof(ScheduleApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ScheduleHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Schedule";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ScheduleApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
