using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(EmergApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class EmergHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Emerg";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(EmergApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
