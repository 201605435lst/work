using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement;
using SnAbp.Users;

namespace SnAbp.Identity
{
    [DependsOn(
        typeof(SnAbpIdentityDomainSharedModule),
        typeof(SnAbpUsersAbstractionModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationModule),
        typeof(SnAbpPermissionManagementApplicationContractsModule)
        )]
    public class AbpIdentityApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}