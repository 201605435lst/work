using System;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent
{
	/// <summary>
	/// 施工计划详情 UpdateDto (更新Dto) 
	/// </summary>
	public class MasterPlanContentUpdateDto 
	{

		/// <summary>
		/// 施工总体计划Id 
		/// </summary>
		public String MasterPlanId {get;set;} 

		/// <summary>
		/// 计划名称
		/// </summary>
		public String Name {get;set;} 

		/// <summary>
		/// 计划描述(批量保存的时候用)
		/// </summary>
		public String Content {get;set;} 

		/// <summary>
		/// 计划开始时间
		/// </summary>
		public DateTime PlanStartTime {get;set;} 

		/// <summary>
		/// 计划结束时间
		/// </summary>
		public DateTime PlanEndTime {get;set;} 

		/// <summary>
		/// 工期
		/// </summary>
		public double Period {get;set;} 

		/// <summary>
		/// 父级id
		/// </summary>
		public String ParentId {get;set;} 
		/// <summary>
		/// 是否历程碑 
		/// </summary>
		public bool IsMilestone { get; set; }
	}
}
