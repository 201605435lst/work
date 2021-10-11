using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using SnAbp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.FeatureManagement
{
    [DependsOn(
        typeof(SnAbpFeatureManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class SnAbpFeatureManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpFeatureManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpFeatureManagementResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
