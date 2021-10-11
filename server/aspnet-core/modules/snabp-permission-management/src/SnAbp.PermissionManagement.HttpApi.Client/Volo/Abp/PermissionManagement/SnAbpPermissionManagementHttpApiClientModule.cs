using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement
{
    [DependsOn(
        typeof(SnAbpPermissionManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class AbpPermissionManagementHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(SnAbpPermissionManagementApplicationContractsModule).Assembly,
                PermissionManagementRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
