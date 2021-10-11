using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(CommonApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class CommonHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Common";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CommonApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
