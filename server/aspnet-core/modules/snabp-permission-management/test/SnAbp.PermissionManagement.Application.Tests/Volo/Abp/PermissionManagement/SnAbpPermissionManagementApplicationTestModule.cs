using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;

namespace SnAbp.PermissionManagement
{
    [DependsOn(
        typeof(SnAbpPermissionManagementApplicationModule),
        typeof(AbpPermissionManagementTestModule)
    )]
    public class AbpPermissionManagementApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();

            context.Services.Configure<PermissionManagementOptions>(options =>
            {
                options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = UserPermissionValueProvider.ProviderName;
                options.ProviderPolicies["Test"] = "Test";
                options.ManagementProviders.Add<TestPermissionManagementProvider>();
            });
        }
    }
}
