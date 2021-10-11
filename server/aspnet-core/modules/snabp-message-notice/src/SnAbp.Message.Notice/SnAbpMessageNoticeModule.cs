using System;

using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Core;
using SnAbp.Message.Service;

using Volo.Abp;
using Volo.Abp.Modularity;

namespace SnAbp.Message.Notice
{
    public class SnAbpMessageNoticeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            // 配置当前模块的客户端名称
            context.Services.AddMessageCore(b => b.UseHttpClient(o =>
                o.Clients.Add(new HttpClientConfig(typeof(NoticeMessage), MessageBaseDefine.MessageType))));
            context.Services.AddSingleton<IMessageNoticeProvider, MessageNoticeProvider>();
        }
    }
}
