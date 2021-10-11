using Localization.Resources.AbpUi;
using SnAbp.Regulation.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Regulation
{
    [DependsOn(
        typeof(RegulationApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class RegulationHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(RegulationHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<RegulationResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
