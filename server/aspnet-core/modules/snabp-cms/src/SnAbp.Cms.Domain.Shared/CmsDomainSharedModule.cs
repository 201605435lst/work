using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Cms.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Cms
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CmsDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsDomainSharedModule>("SnAbp.Cms");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<CmsResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Cms");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Cms", typeof(CmsResource));
            });
        }
    }
}
