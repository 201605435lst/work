using System;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos.Plan.Plan
{
	/// <summary>
	/// 审批流程 Dto
	/// </summary>
	public class PlanProcessDto
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
		public ConstructionPlanState Status { get; set; }
	}
}