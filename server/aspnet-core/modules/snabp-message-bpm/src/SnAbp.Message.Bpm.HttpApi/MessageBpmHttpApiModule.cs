using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using SnAbp.Message.Bpm.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Service;
using Volo.Abp;
using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Message.Bpm
{
    [DependsOn(
        typeof(MessageHttpApiModule),
        typeof(BpmDomainModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(BpmApplicationContractsModule))]
    public class MessageBpmHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSnabpSignalR(b =>
            {
                b.AddHubConfig<BpmHub>(opt => opt.HubRoute = MessageBaseDefine.HubRoute);
                b.AddServiceProvider<BpmServiceProvider>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MessageBpmResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
