using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using SnAbp.TenantManagement.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.TenantManagement
{
    [DependsOn(typeof(AbpValidationModule))]
    public class SnAbpTenantManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpTenantManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpTenantManagementResource>("en")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/Volo/Abp/TenantManagement/Localization/Resources");
            });
        }
    }
}
