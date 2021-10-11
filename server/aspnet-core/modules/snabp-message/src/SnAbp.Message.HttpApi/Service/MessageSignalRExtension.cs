using System;
using Microsoft.Extensions.DependencyInjection;

namespace SnAbp.Message.Service
{
    /// <summary>
    ///     扩展
    /// </summary>
    public static class MessageSignalRExtension
    {
        public static void AddSnabpSignalR(this IServiceCollection serviceCollection,
            Action<MessageSignalRServerBuilder> builder)
        {
            var messageCore = new MessageSignalRServerBuilder(serviceCollection);
            builder.Invoke(messageCore);
        }
    }
}