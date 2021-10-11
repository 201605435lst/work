using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class OaHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Oa";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(OaApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
