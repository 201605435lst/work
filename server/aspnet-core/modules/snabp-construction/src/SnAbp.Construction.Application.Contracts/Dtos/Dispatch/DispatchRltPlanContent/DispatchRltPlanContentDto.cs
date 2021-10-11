using SnAbp.Construction.Dtos.Plan.PlanContent;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联施工计划
    /// </summary>
    public class DispatchRltPlanContentDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanContentId { get; set; }
        public virtual PlanContentDto PlanContent { get; set; }
    }
}
