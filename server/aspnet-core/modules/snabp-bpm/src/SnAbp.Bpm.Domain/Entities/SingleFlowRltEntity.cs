/**********************************************************************
*******命名空间： SnAbp.Bpm.Entities
*******类 名 称： SingleFlowRltEntity
*******类 说 明： 单一流程关联实体，其他模式继承使用
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 11:14:39
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// $$
    /// </summary>
    public abstract class SingleFlowRltEntity : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 工作流id
        /// </summary>
        public virtual Guid WorkFlowId { get; set; }
        /// <summary>
        /// 工作流的状态
        /// </summary>
        public virtual WorkflowState State { get; set; }
        /// <summary>
        /// 流程审批描述内容，如反馈的消息等；
        /// </summary>
        public virtual string Content { get; set; }
    }
}
