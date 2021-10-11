using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using SnAbp.PermissionManagement.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.PermissionManagement.HttpApi
{
    [DependsOn(
        typeof(SnAbpPermissionManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class SnAbpPermissionManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SnAbpPermissionManagementHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpPermissionManagementResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
