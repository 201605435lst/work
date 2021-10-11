/**********************************************************************
*******命名空间： SnAbp.Message.Bpm
*******类 名 称： BpmMessageTransferType
*******类 说 明： 工作流消息流向
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/8 17:22:24
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Message.Bpm
{
    public enum BpmMessageType
    {
        /// <summary>
        /// 通知，通知给流程发起者
        /// </summary>
        Notice,
        /// <summary>
        /// 发送，发送给流程的审批者
        /// </summary>
        Approval,
        /// <summary>
        /// 抄送状态
        /// </summary>
        Cc
    }
}
