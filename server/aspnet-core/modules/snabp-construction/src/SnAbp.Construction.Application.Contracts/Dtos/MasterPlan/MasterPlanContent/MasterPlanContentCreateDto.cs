using System;
using System.Collections.Generic;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent
{
	/// <summary>
	/// 施工计划详情 CreateDto (添加Dto) 
	/// </summary>
	public class MasterPlanContentCreateDto 
	{

		/// <summary>
		/// 施工总体计划Id   添加用
		/// </summary>
		public Guid? MasterPlanId {get;set;} 
		/// <summary>
		/// 施工计划Id 标记用(检查计划下的计划详情是不是有重复任务名) 
		/// </summary>
		public Guid? MasterPlanIdMark {get;set;} 

		/// <summary>
		/// 计划名称
		/// </summary>
		public string Name {get;set;}

		public string Desc { get; set; }

		/// <summary>
		/// 计划描述
		/// </summary>
		public string Content {get;set;} 

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
		public string ParentId {get;set;} 
		/// <summary>
		/// 是否历程碑 
		/// </summary>
		public bool IsMilestone { get; set; }

		#region 以下是 gantt图 的 属性 ,通过mapper转换 
		

		/// <summary>
		/// 计划开始时间(批量保存的时候用)
		/// </summary>
		public string StartDate {get;set;} 

		/// <summary>
		/// 计划结束时间(批量保存的时候用)
		/// </summary>
		public string EndDate {get;set;} 

		/// <summary>
		/// 工期(批量保存的时候用)
		/// </summary>
		public double Duration {get;set;} 


		/// <summary>
		/// 前置任务 ids 
		/// </summary>
		public List<Guid> PreTaskIds { get; set; } = new List<Guid>();
		

		#endregion
	}
}
