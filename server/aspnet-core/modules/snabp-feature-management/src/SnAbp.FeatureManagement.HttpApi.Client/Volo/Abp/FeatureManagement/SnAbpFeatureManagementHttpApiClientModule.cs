using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class SnAbpFeatureManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SnAbpFeatureManagementApplicationContractsModule).Assembly,
                FeatureManagementRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
