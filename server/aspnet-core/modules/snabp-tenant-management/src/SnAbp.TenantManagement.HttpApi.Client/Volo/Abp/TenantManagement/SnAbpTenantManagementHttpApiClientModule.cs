using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.TenantManagement
{
    [DependsOn(
        typeof(SnAbpTenantManagementApplicationContractsModule), 
        typeof(AbpHttpClientModule))]
    public class SnAbpTenantManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SnAbpTenantManagementApplicationContractsModule).Assembly,
                TenantManagementRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}