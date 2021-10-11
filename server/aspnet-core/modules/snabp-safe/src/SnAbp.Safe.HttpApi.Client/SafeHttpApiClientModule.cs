using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SafeHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Safe";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SafeApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
