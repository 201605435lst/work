using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class TechnologyHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Technology";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TechnologyApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
