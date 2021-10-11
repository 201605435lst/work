using Microsoft.Extensions.DependencyInjection;
using SnAbp.Account;
using SnAbp.FeatureManagement;
using SnAbp.Identity;
using SnAbp.PermissionManagement;
using SnAbp.TenantManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(SnAbpAccountHttpApiClientModule),
        typeof(SnAbpIdentityHttpApiClientModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(SnAbpTenantManagementHttpApiClientModule),
        typeof(SnAbpFeatureManagementHttpApiClientModule)
    )]
    public class MyProjectNameHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(MyProjectNameApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
