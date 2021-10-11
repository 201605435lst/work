using System;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SnAbp.Message.Hubs;
using SnAbp.Message.Message;
using SnAbp.Message.MessageDefine;
using Volo.Abp.AspNetCore.SignalR;

namespace SnAbp.Message.Service
{
    /// <summary>
    ///     SignalR 服务
    /// </summary>
    public class MessageSignalRServerBuilder
    {
        public MessageSignalRServerBuilder(
            IServiceCollection serviceCollection) => Service = serviceCollection;

        IServiceCollection Service { get; }

        /// <summary>
        ///     注册signalr 服务
        /// </summary>
        /// <typeparam name="THub"></typeparam>
        /// <param name="options"></param>
        public void AddHubConfig<THub>(Action<SignalROption> options) where THub : AbpHub
        {
            var option = new SignalROption();
            options.Invoke(option);
            Service.AddTransient<THub>(); // 注入服务
            Service.Configure<AbpSignalROptions>(opt => opt.Hubs.AddOrUpdate(typeof(THub),
                config =>
                {
                    config.RoutePattern = option.HubRoute;
                    config.ConfigureActions.Add(hubOption =>
                        {
                            hubOption.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
                            hubOption.Transports=HttpTransportType.LongPolling;
                        });
                }));
            Service.AddSignalR()
                .AddJsonProtocol(opt => opt.PayloadSerializerOptions.PropertyNamingPolicy = null);
            // 配置消息上下文服务
            Service.AddTransient(serviceType: typeof(IMessageContext<>), implementationType: typeof(MessageContext<>));
        }

        public void AddMessageEntity<TMessage>(Type type) where TMessage : BaseMessage
        {
            Service.TryAddSingleton(type);
        }

        public void AddServiceProvider<T>()
        {
            Service.AddSingleton(serviceType: typeof(IMessageServiceProvider), implementationType: typeof(T));
        }
    }
}