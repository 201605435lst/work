/**********************************************************************
*******命名空间： SnAbp.Message.Service
*******接口名称： IHttpClientProvider
*******接口说明： 客户端提供者，具体的接口定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/12 17:12:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SnAbp.Message.Core
{
    /// <summary>
    ///     http请求对象
    /// </summary>
    public interface IHttpClientProvider : IDomainService
    {
        Task PostAsync<T>(byte[] data);
        Task PostAsync<T>(string route,byte[] data);
    }
}