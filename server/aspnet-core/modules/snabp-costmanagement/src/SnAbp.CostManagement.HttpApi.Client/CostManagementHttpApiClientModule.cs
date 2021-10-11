using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.CostManagement
{
    [DependsOn(
        typeof(CostManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class CostManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CostManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CostManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
