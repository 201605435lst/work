using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.StdBasic.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.StdBasic
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class StdBasicDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StdBasicDomainSharedModule>("SnAbp.StdBasic");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<StdBasicResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/StdBasic");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("StdBasic", typeof(StdBasicResource));
            });
        }
    }
}
