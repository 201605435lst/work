using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Material
{
    [DependsOn(
        typeof(MaterialApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class MaterialHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Material";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MaterialApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
