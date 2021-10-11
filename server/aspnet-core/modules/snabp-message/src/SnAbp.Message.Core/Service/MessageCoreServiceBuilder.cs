using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SnAbp.Message.Core;

namespace SnAbp.Message
{
    /// <summary>
    ///     消息中心服务构建类，封装基础层得服务注册机制
    /// </summary>
    public class MessageCoreServiceBuilder
    {

        IServiceCollection Service { get; }

        public MessageCoreServiceBuilder(
            IServiceCollection serviceCollection) =>
            Service = serviceCollection;

        public void UseHttpClient(Action<HttpClientOptions> options)
        {
            Service.Configure(options);
            Service.AddSingleton<IHttpClientProvider,HttpClientProvider>();
        }
    }
}