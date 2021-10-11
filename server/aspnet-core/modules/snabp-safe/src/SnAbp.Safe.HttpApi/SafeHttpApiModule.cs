using Localization.Resources.AbpUi;
using SnAbp.Safe.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Safe
{
    [DependsOn(
        typeof(SafeApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class SafeHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SafeHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<SafeResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
