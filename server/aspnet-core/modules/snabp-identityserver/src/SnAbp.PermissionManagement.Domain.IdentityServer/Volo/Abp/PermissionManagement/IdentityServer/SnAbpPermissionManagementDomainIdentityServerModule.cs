using Volo.Abp.Authorization.Permissions;
using SnAbp.IdentityServer;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement.IdentityServer
{
    [DependsOn(
        typeof(SnAbpIdentityServerDomainSharedModule),
        typeof(SnAbpPermissionManagementDomainModule)
    )]
    public class SnAbpPermissionManagementDomainIdentityServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<ClientPermissionManagementProvider>();

                options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
            });
        }
    }
}
