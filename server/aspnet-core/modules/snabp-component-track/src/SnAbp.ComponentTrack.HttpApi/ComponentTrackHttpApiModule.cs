using Localization.Resources.AbpUi;
using SnAbp.ComponentTrack.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.ComponentTrack
{
    [DependsOn(
        typeof(ComponentTrackApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ComponentTrackHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ComponentTrackHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ComponentTrackResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
