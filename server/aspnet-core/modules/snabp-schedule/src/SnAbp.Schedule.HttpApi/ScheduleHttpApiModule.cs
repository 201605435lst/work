using Localization.Resources.AbpUi;
using SnAbp.Schedule.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Schedule
{
    [DependsOn(
        typeof(ScheduleApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ScheduleHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ScheduleHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ScheduleResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
