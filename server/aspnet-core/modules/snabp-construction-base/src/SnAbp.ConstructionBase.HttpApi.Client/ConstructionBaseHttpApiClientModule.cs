using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.ConstructionBase
{
    [DependsOn(
        typeof(ConstructionBaseApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ConstructionBaseHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "ConstructionBase";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ConstructionBaseApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
