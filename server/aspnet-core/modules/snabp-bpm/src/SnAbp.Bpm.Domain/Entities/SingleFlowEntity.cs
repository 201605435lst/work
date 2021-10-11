/**********************************************************************
*******命名空间： SnAbp.Bpm.Entities
*******类 名 称： SingleFlowEntity
*******类 说 明： 单一流程模板，用于其他模板继承使用，只有流程，没有表单的流程
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 11:04:08
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// 单一流程模板
    /// </summary>
    public class SingleFlowEntity : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 工作流id
        /// </summary>
        public virtual Guid? WorkflowId { get; set; }
        /// <summary>
        /// 工作流模板id
        /// </summary>
        public virtual Guid? WorkflowTemplateId { get; set; }
        /// <summary>
        /// 工作流
        /// </summary>
        public virtual Workflow Workflow { get; set; }
        /// <summary>
        /// 工作流模板
        /// </summary>
        public virtual WorkflowTemplate WorkflowTemplate { get; set; }
    }
}
