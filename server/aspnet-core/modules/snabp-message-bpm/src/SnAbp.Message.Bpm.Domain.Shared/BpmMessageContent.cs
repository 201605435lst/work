/**********************************************************************
*******命名空间： SnAbp.Message.Notice
*******类 名 称： NoticeMessageContent
*******类 说 明： 通知消息内容实体定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 16:21:48
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Message.MessageDefine;

namespace SnAbp.Message.Bpm
{
    [Serializable]
    public class BpmMessageContent : MessageContent
    {
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 工作流的id
        /// </summary>
        public Guid WorkFlowId { get; set; }
        /// <summary>
        /// 接收消息得用户
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 流程的发起人id
        /// </summary>
        public Guid SponsorId { get; set; }
        /// <summary>
        /// 处理者id
        /// </summary>
        public Guid ProcessorId { get; set; }
        /// <summary>
        /// 工作流的状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 消息流转方向
        /// </summary>
        public BpmMessageType Type { get; set; }
    }
}
