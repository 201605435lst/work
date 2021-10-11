using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.CrPlan.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.CrPlan
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CrPlanDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CrPlanDomainSharedModule>("SnAbp.CrPlan");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<CrPlanResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/CrPlan");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("CrPlan", typeof(CrPlanResource));
            });
        }
    }
}
