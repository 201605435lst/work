using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.SettingManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.SettingManagement
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class SnAbpSettingManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SnAbpSettingManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpSettingManagementResource>("en")
                    .AddVirtualJson("/Volo/Abp/SettingManagement/Localization/Resources/AbpSettingManagement");
            });
        }
    }
}
