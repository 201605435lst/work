using System;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Message.Service
{
    public static class MessageCoreServiceExtensions
    {
        /// <summary>
        ///     添加消息中心服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="builder"></param>
        public static void AddMessageCore(this IServiceCollection serviceCollection,
            Action<MessageCoreServiceBuilder> builder) => builder.Invoke(new MessageCoreServiceBuilder(serviceCollection));
    }
}