using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Message.MessageDefine;

namespace SnAbp.Message.Bpm
{
    /// <summary>
    ///  当前消息类型定义，一个模块消息类型只有一个
    /// </summary>
    public class MessageBaseDefine
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public const string MessageType = "bpm";

        /// <summary>
        /// 服务路由
        /// </summary>
        public const string HubRoute = MessageRoute.BaseRoute + MessageType;
    }
}
