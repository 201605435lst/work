using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.PermissionManagement
{
    [DependsOn(
        typeof(AbpValidationModule)
        )]
    public class SnAbpPermissionManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpPermissionManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpPermissionManagementResource>("en")
                    .AddBaseTypes(
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/Volo/Abp/PermissionManagement/Localization/Domain");
            });
        }
    }
}
