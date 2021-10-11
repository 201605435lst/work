/**********************************************************************
*******命名空间： SnAbp.Message.Service
*******接口名称： HttpClientOption
*******接口说明： 客户端配置类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/12 17:12:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using JetBrains.Annotations;

namespace SnAbp.Message.Core
{
    /// <summary>
    ///  客户端配置类
    /// </summary>
    public class HttpClientConfig
    {

        [NotNull] public Type ClientType { get; set; }
        public string RequestType { get; set; }
        public HttpClientConfig([NotNull]Type clientType,[NotNull]string requestType)
        {
            this.RequestType = requestType;
            this.ClientType = clientType;
        }
    }
}