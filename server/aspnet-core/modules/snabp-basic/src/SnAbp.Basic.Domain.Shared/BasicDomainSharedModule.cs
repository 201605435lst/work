using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Basic.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Basic
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class BasicDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BasicDomainSharedModule>("SnAbp.Basic");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BasicResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Basic");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Basic", typeof(BasicResource));
            });
        }
    }
}
