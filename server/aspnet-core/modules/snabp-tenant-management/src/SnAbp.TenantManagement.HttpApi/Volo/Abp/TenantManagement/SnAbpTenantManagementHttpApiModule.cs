using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using SnAbp.FeatureManagement;
using SnAbp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using SnAbp.TenantManagement.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.TenantManagement
{
    [DependsOn(
        typeof(SnAbpTenantManagementApplicationContractsModule),
        typeof(SnAbpFeatureManagementHttpApiModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class SnAbpTenantManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpTenantManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpTenantManagementResource>()
                    .AddBaseTypes(
                        typeof(AbpFeatureManagementResource),
                        typeof(AbpUiResource));
            });
        }
    }
}