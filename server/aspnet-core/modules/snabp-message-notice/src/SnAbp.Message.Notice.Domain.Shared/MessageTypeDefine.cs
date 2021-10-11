using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Message.MessageDefine;

namespace SnAbp.Message.Notice
{
    /// <summary>
    ///  当前消息类型定义，一个模块消息类型只有一个
    /// </summary>
    public class MessageBaseDefine
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public const string MessageType = "notice";

        /// <summary>
        /// 服务路由
        /// </summary>
        public const string HubRoute = MessageRoute.BaseRoute + MessageType;

        /// <summary>
        /// 文件状态改变后通知路由定义
        /// </summary>
        public const string FileNoticeRoute = MessageRoute.BaseRoute + "file";
        /// <summary>
        /// 多文件改变后提示
        /// </summary>
        public const string FilesNoticeRoute = MessageRoute.BaseRoute + "files";
    }
}
