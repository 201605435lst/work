using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// 工作流表单记录表
    /// </summary>
    public class WorkflowData : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected WorkflowData() { }

        public WorkflowData(Guid id)
        {
            Id = id;
        }


        /// <summary>
        /// 工作流 Id
        /// </summary>
        public Guid WorkflowId { get; set; }


        /// <summary>
        /// 工作流
        /// </summary>
        public virtual Workflow Workflow { get; set; }


        /// <summary>
        /// 表单值
        /// </summary>
        public virtual string FormValues { get; set; }


        /// <summary>
        /// 起始节点 Id
        /// </summary>
        public Guid? SourceNodeId { get; set; }


        /// <summary>
        /// 目标节点 Id
        /// </summary>
        public Guid? TargetNodeId { get; set; }


        /// <summary>
        /// 流程状态
        /// </summary>
        public WorkflowStepState? StepState { get; set; }


        /// <summary>
        /// 审批意见
        /// </summary>
        public string Comments { get; set; }
    }
}
