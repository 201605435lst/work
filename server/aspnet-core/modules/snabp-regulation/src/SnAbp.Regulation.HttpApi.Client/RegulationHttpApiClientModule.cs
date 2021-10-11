using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class RegulationHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Regulation";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(RegulationApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
