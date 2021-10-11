using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Bpm.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Bpm
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class BpmDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BpmDomainSharedModule>("SnAbp.Bpm");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BpmResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Bpm");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Bpm", typeof(BpmResource));
            });
        }
    }
}
