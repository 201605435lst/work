using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SnAbp.Message.Middleware;
using SnAbp.Message.Notice.Localization;
using SnAbp.Message.Service;
using Volo.Abp;

namespace SnAbp.Message.Notice
{
    [DependsOn(
        typeof(MessageHttpApiModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class NoticeHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(NoticeHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSnabpSignalR(b =>
                {
                    b.AddHubConfig<NoticeHub>(opt => opt.HubRoute = MessageBaseDefine.HubRoute);
                    b.AddServiceProvider<NoticeServiceProvider>();
                });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            // 使用消息中间件
            app.UseSnAbpMessageCore(option => option.MessageType = MessageBaseDefine.MessageType);
        }
    }
}
