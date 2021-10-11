using Localization.Resources.AbpUi;
using SnAbp.Alarm.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Service;
using Volo.Abp;
using SnAbp.Message;

namespace SnAbp.Alarm
{
    [DependsOn(
        typeof(AlarmApplicationContractsModule),
        typeof(MessageHttpApiModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AlarmHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AlarmHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSnabpSignalR(b =>
            {
                b.AddHubConfig<AlarmHub>(opt => opt.HubRoute ="message/alarm");
            });

            context.Services.AddSingleton(typeof(AlarmServerProvider));

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AlarmResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var serviceP = context.ServiceProvider;
            var alarm = serviceP.GetService<AlarmServerProvider>();
            alarm.Start();
        }
    }
}
