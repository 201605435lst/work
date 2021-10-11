using Localization.Resources.AbpUi;
using SnAbp.Project.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Project
{
    [DependsOn(
        typeof(ProjectApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ProjectHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ProjectHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ProjectResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
