using System;

using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.Core;
using SnAbp.Message.Service;

using Volo.Abp.Modularity;

namespace SnAbp.Message.Bpm
{
    public class SnAbpMessageBpmModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 配置当前模块的客户端名称
            context.Services.AddMessageCore(builder =>
                builder.UseHttpClient(option => option.Clients.Add(new HttpClientConfig(typeof(BpmMessage), MessageBaseDefine.MessageType))));
            context.Services.AddSingleton<IMessageBpmProvider, MessageBpmProvider>();
        }
    }
}
