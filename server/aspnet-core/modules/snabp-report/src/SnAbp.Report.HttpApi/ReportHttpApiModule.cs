using Localization.Resources.AbpUi;
using SnAbp.Report.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Report
{
    [DependsOn(
        typeof(ReportApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ReportHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ReportHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ReportResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
