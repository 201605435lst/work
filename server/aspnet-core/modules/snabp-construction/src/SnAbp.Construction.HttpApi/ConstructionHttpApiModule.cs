using Localization.Resources.AbpUi;
using SnAbp.Construction.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Construction
{
    [DependsOn(
        typeof(ConstructionApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ConstructionHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ConstructionHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ConstructionResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
