using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ConstructionHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Construction";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ConstructionApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
