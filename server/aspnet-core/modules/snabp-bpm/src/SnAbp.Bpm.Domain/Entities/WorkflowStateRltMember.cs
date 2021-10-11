using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Bpm.Entities
{
    public class WorkflowStateRltMember : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        protected WorkflowStateRltMember() { }

        public WorkflowStateRltMember(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 成员Id
        /// </summary>
        public Guid MemberId { get; set; }

        //public Member Member { get; set; }
        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType MemberType { get; set; }

        /// <summary>
        /// 状态分组
        /// </summary>
        public UserWorkflowGroup Group { get; set; }

        /// <summary>
        /// 工作流处理状态
        /// </summary>
        public WorkflowState State { get; set; }
        /// <summary>
        /// 工作流Id
        /// </summary>
        public Guid WorkflowId { get; set; }

        public virtual Workflow Workflow { get; set; }

        /// <summary>
        /// 简报
        /// </summary>
        //public Guid WorkflowDataId { get; set; }

        //public virtual WorkflowData WorkflowData { get; set; }
    }
}
