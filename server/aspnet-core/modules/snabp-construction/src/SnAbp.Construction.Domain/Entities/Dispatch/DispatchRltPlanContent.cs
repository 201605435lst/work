using SnAbp.Construction.Plans;
using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联施工计划
    /// </summary>
    public class DispatchRltPlanContent : Entity<Guid>
    {
        public DispatchRltPlanContent(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }

        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanContentId { get; set; }
        public virtual PlanContent PlanContent { get; set; }
    }
}
