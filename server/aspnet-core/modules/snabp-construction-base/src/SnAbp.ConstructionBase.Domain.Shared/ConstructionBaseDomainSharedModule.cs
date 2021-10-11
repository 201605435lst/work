using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.ConstructionBase.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.ConstructionBase
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ConstructionBaseDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ConstructionBaseDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ConstructionBaseResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/ConstructionBase");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("ConstructionBase", typeof(ConstructionBaseResource));
            });
        }
    }
}
