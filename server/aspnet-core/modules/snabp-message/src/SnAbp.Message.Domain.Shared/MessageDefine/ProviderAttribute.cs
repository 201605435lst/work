/**********************************************************************
*******命名空间： SnAbp.Message.MessageDefine
*******类 名 称： MessageType
*******类 说 明： 消息类型枚举定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 15:13:19
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;

namespace SnAbp.Message.MessageDefine
{
    public class MessageServiceProviderAttribute : Attribute
    {
        public MessageServiceProviderAttribute(string providerName) => _name = providerName;
        protected string _name { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name => this._name;
    }
}