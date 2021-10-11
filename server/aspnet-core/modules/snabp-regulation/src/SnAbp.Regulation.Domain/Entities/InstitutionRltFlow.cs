using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

using SnAbp.Bpm.Entities;
using SnAbp.Identity;
using SnAbp.Oa.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Regulation.Entities
{
    public class InstitutionRltFlow : AuditedEntity<Guid>
    {
        protected InstitutionRltFlow() { }
        public InstitutionRltFlow(Guid id) { Id = id;}

        public virtual Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual Guid WorkFlowId { get; set; }
        public virtual Workflow WorkFlow { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApproveTime{get;set;}

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApproveState { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public IdentityUser Creator { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Suggestion { get; set; }

        /// <summary>
        /// 电子签章
        /// </summary>
        public virtual Guid? SealId { get; set; }
        [CanBeNull] public virtual Seal Seal { get; set; }
    }
}
