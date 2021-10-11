using MyCompanyName.MyProjectName.Localization;
using SnAbp.BackgroundJobs;
using SnAbp.FeatureManagement;
using SnAbp.Identity;
using SnAbp.IdentityServer;
using SnAbp.PermissionManagement;
using SnAbp.SettingManagement;
using SnAbp.TenantManagement;
using Volo.Abp.AuditLogging;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainSharedModule),
        typeof(SnAbpFeatureManagementDomainSharedModule),
        typeof(SnAbpIdentityDomainSharedModule),
        typeof(SnAbpIdentityServerDomainSharedModule),
        typeof(SnAbpPermissionManagementDomainSharedModule),
        typeof(SnAbpSettingManagementDomainSharedModule),
        typeof(SnAbpTenantManagementDomainSharedModule)
        )]
    public class MyProjectNameDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            MyProjectNameModulePropertyConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MyProjectNameResource>("zh-Hans")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/MyProjectName");
                
                options.DefaultResourceType = typeof(MyProjectNameResource);
            });
        }
    }
}
