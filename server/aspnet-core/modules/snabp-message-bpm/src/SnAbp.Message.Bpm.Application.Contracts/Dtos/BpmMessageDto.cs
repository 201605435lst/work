/**********************************************************************
*******命名空间： SnAbp.Message.Bpm.Dtos
*******类 名 称： BpmMessageDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/11 14:21:00
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Message.Bpm.Dtos
{
    public class BpmMessageDto
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        /// 流程id
        /// </summary>
        public Guid FlowId { get; set; }
        /// <summary>
        /// 信息是否已读
        /// </summary>
        public bool Process { get; set; }

        /// <summary>
        /// 流程发起人
        /// </summary>
        public string Sponsor { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string Processor { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public  DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息的类型，例如发送给谁，消息的流向
        /// </summary>
        public BpmMessageType Type { get; set; }
        /// <summary>
        /// 工作流状态
        /// </summary>
        public int State { get; set; }
    }
}
