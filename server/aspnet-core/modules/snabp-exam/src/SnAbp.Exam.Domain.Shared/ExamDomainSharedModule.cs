using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using SnAbp.Exam.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace SnAbp.Exam
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ExamDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ExamDomainSharedModule>("SnAbp.Exam");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ExamResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Exam");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Exam", typeof(ExamResource));
            });
        }
    }
}
