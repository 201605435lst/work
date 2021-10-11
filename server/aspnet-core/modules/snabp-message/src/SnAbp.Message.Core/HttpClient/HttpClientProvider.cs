/**********************************************************************
*******命名空间： SnAbp.Message.Service
*******接口名称： HttpClientProvider
*******接口说明： 客户端提供者，具体的请求实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/12 17:12:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Message.MessageDefine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SnAbp.Message.Core
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public HttpClient Client { get; }
        public IServiceProvider ServiceProvider { get; }
        public HttpClientProvider(HttpClient client, IServiceProvider service)
        {
            ServiceProvider = service;
            var config = service.GetService<IConfiguration>();
            client.BaseAddress = new Uri(config["App:SelfUrl"]);
            Client = client;
        }
        /// <summary>
        /// 发送数据请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task PostAsync<T>(byte[] data)
        {
            var config = GetClientConfig(typeof(T));
            var byteContent = new ByteArrayContent(data);
            await Client.PostAsync(requestUri: $"/{config.RequestType}{MessageRoute.RouteTopic}", byteContent);
        }
        public async Task PostAsync<T>(string route,byte[] data)
        {
            var config = GetClientConfig(typeof(T));
            var byteContent = new ByteArrayContent(data);
            await Client.PostAsync(requestUri: route, byteContent);
        }
        protected HttpClientConfig GetClientConfig(Type type)
        {
            var options = ServiceProvider.GetRequiredService<IOptions<HttpClientOptions>>().Value;
            return options.Clients.FirstOrDefault(a => a.ClientType == type);
        }
    }
}