using System;
using Microsoft.AspNetCore.Builder;

namespace SnAbp.Message.Middleware
{
    public static class MessageCoreMiddlewareExtensions
    {
        public static IApplicationBuilder UseSnAbpMessageCore(this IApplicationBuilder app,
            Action<MessageHttpOption> options)
        {
            var option = new MessageHttpOption();
            options.Invoke(option);
            return app.UseMiddleware<MessageCoreMiddleware>();
        }
    }
}