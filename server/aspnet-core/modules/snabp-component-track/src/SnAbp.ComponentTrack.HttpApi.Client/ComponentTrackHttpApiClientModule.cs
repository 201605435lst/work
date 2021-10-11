using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.ComponentTrack
{
    [DependsOn(
        typeof(ComponentTrackApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class ComponentTrackHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "ComponentTrack";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ComponentTrackApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
