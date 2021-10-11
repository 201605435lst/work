using Localization.Resources.AbpUi;
using SnAbp.Material.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Material
{
    [DependsOn(
        typeof(MaterialApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class MaterialHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(MaterialHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MaterialResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
