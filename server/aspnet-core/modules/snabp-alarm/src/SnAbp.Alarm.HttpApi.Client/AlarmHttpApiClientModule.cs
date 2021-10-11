using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AlarmHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Alarm";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AlarmApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
