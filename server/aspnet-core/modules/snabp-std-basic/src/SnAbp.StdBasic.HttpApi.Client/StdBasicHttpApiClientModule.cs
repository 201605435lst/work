using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(StdBasicApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class StdBasicHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "StdBasic";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(StdBasicApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
