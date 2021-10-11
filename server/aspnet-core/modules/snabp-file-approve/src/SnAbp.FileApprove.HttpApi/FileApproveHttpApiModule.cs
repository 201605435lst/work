using Localization.Resources.AbpUi;
using SnAbp.FileApprove.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.FileApprove
{
    [DependsOn(
        typeof(FileApproveApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class FileApproveHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(FileApproveHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<FileApproveResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
