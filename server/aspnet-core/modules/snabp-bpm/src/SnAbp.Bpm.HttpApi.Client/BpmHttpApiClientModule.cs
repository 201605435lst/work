using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(BpmApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class BpmHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Bpm";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BpmApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
