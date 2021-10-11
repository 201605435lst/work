using Localization.Resources.AbpUi;
using SnAbp.Technology.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Technology
{
    [DependsOn(
        typeof(TechnologyApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class TechnologyHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(TechnologyHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<TechnologyResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
