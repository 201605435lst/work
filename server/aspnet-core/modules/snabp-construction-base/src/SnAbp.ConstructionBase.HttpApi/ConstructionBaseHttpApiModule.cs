using Localization.Resources.AbpUi;
using SnAbp.ConstructionBase.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.ConstructionBase
{
    [DependsOn(
        typeof(ConstructionBaseApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ConstructionBaseHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ConstructionBaseHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ConstructionBaseResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
