using System;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlan
{
	/// <summary>
	/// 施工计划 UpdateDto (更新Dto) 
	/// </summary>
	public class MasterPlanUpdateDto 
	{
		/// <summary>
		/// 负责人Id 
		/// </summary>
		 public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

		/// <summary>
		/// 计划名称
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 计划名称
		/// </summary>
		public String Content { get; set; }

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime PlanStartTime { get; set; }

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime PlanEndTime { get; set; }

		/// <summary>
		/// 计划工期
		/// </summary>
		public double Period { get; set; }

		/// <summary>
		/// 负责人Id 
		/// </summary>
		public Guid? ChargerId { get; set; }
		/// <summary>
		/// 是否历程碑
		/// </summary>
		public bool IsMilestone {get;set;}

		/// <summary>
		/// 审批流程状态
		/// </summary>
		public MasterPlanState State { get; set; }

	}
}
