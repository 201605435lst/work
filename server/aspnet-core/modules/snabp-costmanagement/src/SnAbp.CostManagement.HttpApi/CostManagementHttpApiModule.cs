using Localization.Resources.AbpUi;
using SnAbp.CostManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.CostManagement
{
    [DependsOn(
        typeof(CostManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class CostManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CostManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<CostManagementResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
