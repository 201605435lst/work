using System;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos.Plan.Plan
{
	/// <summary>
	/// 施工计划 CreateDto (添加Dto) 
	/// </summary>
	public class PlanCreateDto 
	{

		/// <summary>
		/// 总体计划Id 
		/// </summary>
		public Guid? MasterPlanId {get;set;}
		/// <summary>
		/// 计划名称
		/// </summary>
		public String Name {get;set;} 
		/// <summary>
		/// 计划描述
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
		/// 计划工期
		/// </summary>
		public double Period {get;set;}
		/// <summary>
		/// 负责人Id 
		/// </summary>
		public Guid? ChargerId {get;set;}
		/// <summary>
		/// 审批流程状态
		/// </summary>
		public MasterPlanState State {get;set;}= MasterPlanState.ToSubmit; 
	}
}
