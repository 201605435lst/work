using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Resource.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Resource
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ResourceDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ResourceDomainSharedModule>("SnAbp.Resource");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ResourceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Resource");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Resource", typeof(ResourceResource));
            });
        }
    }
}
