using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联施工计划
    /// </summary>
    public class DispatchRltPlanContentCreateDto
    {
        /// <summary>
        /// 施工计划
        /// </summary>
        public virtual Guid PlanContentId { get; set; }
    }
}
