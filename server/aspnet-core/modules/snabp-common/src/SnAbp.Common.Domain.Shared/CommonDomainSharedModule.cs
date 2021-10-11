using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Common.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Common
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CommonDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CommonDomainSharedModule>("SnAbp.Common");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<CommonResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Common");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Common", typeof(CommonResource));
            });
        }
    }
}
