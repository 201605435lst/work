/**********************************************************************
*******命名空间： SnAbp.Message.Bpm.Entities
*******类 名 称： BpmRltMessage
*******类 说 明： 工作流，消息关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/8 16:08:49
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using JetBrains.Annotations;

using SnAbp.Bpm;
using SnAbp.Bpm.Entities;
using SnAbp.Identity;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Message.Bpm.Entities
{
    public sealed class BpmRltMessage : AuditedEntity<Guid>
    {
        public BpmRltMessage(Guid id) => Id = id;
        /// <summary>
        /// 接收消息的用户
        /// </summary>
        public Guid UserId { get; set; }
        public IdentityUser User { get; set; }
        /// <summary>
        /// 工作流的信息
        /// </summary>
        public Guid WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        /// <summary>
        /// 工作流的状态
        /// </summary>
        public WorkflowState State { get; set; }
        /// <summary>
        /// 流程处理人的id
        /// </summary>
        public Guid ProcessorId { get; set; }
        public IdentityUser Processor { get; set; }

        /// <summary>
        /// 流程发起人id
        /// </summary>
        public Guid SponsorId { get; set; }
        public IdentityUser Sponsor { get; set; }
        /// <summary>
        /// 流程的类型
        /// </summary>
        public BpmMessageType Type { get; set; }
        /// <summary>
        /// 流程是否处理或已读
        /// </summary>
        public bool Process { get; set; }
        /// <summary>
        /// 预留的分组
        /// </summary>
        public int Group { get; set; }
    }
}
