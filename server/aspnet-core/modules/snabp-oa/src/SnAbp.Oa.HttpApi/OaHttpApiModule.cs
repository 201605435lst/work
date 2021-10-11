using Localization.Resources.AbpUi;
using SnAbp.Oa.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Oa
{
    [DependsOn(
        typeof(OaApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class OaHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(OaHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OaResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
