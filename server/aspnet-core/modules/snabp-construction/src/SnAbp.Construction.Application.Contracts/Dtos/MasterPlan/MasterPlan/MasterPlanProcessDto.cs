using System;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlan
{
	/// <summary>
	/// 审批流程 Dto
	/// </summary>
	public class MasterPlanProcessDto
	{
        /// <summary>
        /// 任务计划id
        /// </summary>
        public Guid PlanId { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审批状态（pass 和unpass)
        /// </summary>
        public MasterPlanState Status { get; set; }
	}
}