using Volo.Abp.Authorization.Permissions;
using SnAbp.Identity;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement.Identity
{
    [DependsOn(
        typeof(SnAbpIdentityDomainSharedModule),
        typeof(SnAbpPermissionManagementDomainModule)
        )]
    public class SnAbpPermissionManagementDomainIdentityModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<UserPermissionManagementProvider>();
                options.ManagementProviders.Add<RolePermissionManagementProvider>();

                //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
                options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "AbpIdentity.Users.ManagePermissions";
                options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "AbpIdentity.Roles.ManagePermissions";
            });
        }
    }
}