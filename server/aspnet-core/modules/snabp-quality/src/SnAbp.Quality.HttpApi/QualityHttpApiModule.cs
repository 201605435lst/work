using Localization.Resources.AbpUi;
using SnAbp.Quality.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Quality
{
    [DependsOn(
        typeof(QualityApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class QualityHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(QualityHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<QualityResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
