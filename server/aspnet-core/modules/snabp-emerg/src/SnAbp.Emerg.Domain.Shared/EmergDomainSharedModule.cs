using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Emerg.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Emerg
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class EmergDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<EmergDomainSharedModule>("SnAbp.Emerg");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<EmergResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Emerg");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Emerg", typeof(EmergResource));
            });
        }
    }
}
