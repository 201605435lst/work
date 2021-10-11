using MyCompanyName.MyProjectName.MultiTenancy;
using MyCompanyName.MyProjectName.ObjectExtending;
using SnAbp.FeatureManagement;
using SnAbp.Identity;
using SnAbp.IdentityServer;
using SnAbp.PermissionManagement.Identity;
using SnAbp.PermissionManagement.IdentityServer;
using SnAbp.SettingManagement;
using SnAbp.TenantManagement;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(SnAbpFeatureManagementDomainModule),
        typeof(SnAbpIdentityDomainModule),
        typeof(SnAbpPermissionManagementDomainIdentityModule),
        typeof(SnAbpIdentityServerDomainModule),
        typeof(SnAbpPermissionManagementDomainIdentityServerModule),
        typeof(SnAbpSettingManagementDomainModule),
        typeof(SnAbpTenantManagementDomainModule)
        )]
    public class MyProjectNameDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            MyProjectNameDomainObjectExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }
    }
}
