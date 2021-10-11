using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using SnAbp.Message.MessageDefine;

using Volo.Abp.Modularity;

namespace SnAbp.Message.Middleware
{
    public class MessageCoreMiddleware
    {
        readonly RequestDelegate _next;
        readonly IServiceProvider _serviceProvider;
        readonly ServiceConfigurationContext _services;
        public MessageCoreMiddleware(
            RequestDelegate next, ServiceConfigurationContext services, IServiceProvider provider)
        {
            _next = next;
            _services = services;
            _serviceProvider = provider;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            // 处理业务
            var request = httpContext.Request;
            if (request.Path.ToString().Contains(MessageRoute.RouteTopic) && request.ContentLength > 0)
            {
                // 获取二级路由，
                var list = _services.Services.Where(a =>
                    a.Lifetime == ServiceLifetime.Singleton && a.ImplementationType != null);
                foreach (var serviceDescriptor
                    in list)
                {
                    if (!(Attribute.GetCustomAttribute(
                            serviceDescriptor.ImplementationType, 
                            typeof(MessageServiceProviderAttribute))
                        is MessageServiceProviderAttribute attr) || 
                        attr.Name != GetRequestType(request.Path))
                    {
                        continue;
                    }
                    var data = await ReadRequestBody(request.Body);
                    var messageProviders =
                        _serviceProvider.GetServices(typeof(IMessageServiceProvider));
                    foreach (var obj in messageProviders)
                    {
                        if (obj.GetType() != serviceDescriptor.ImplementationType)
                        {
                            continue;
                        }

                        if (obj is IMessageServiceProvider serviceProvider)
                        {
                            await serviceProvider?.SendMessage(data);
                        }
                        break;
                    }
                }
            }
        }

        protected virtual async Task<byte[]> ReadRequestBody(Stream stream)
        {
            using var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            var memoryStream = new MemoryStream();
            await sr.BaseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        private static string GetRequestType(string path) => path.Replace(MessageRoute.RouteTopic, "").Replace("/", "");
    }
}