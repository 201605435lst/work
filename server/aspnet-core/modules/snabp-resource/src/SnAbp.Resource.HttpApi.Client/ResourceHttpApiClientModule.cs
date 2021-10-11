using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(ResourceApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ResourceHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Resource";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ResourceApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
