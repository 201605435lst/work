using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Problem.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Problem
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ProblemDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ProblemDomainSharedModule>("SnAbp.Problem");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ProblemResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Problem");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Problem", typeof(ProblemResource));
            });
        }
    }
}
