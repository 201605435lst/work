using System;
using System.Collections.Generic;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos.Plan.PlanContent
{
	/// <summary>
	/// 施工计划详情(甘特图专用) UpdateDto (更新Dto) 
	/// </summary>
	public class PlanContentGanttUpdateDto 
	{
		/// <summary>
		/// 主键id (如果 GanttItemState 状态是添加的话,就是前端js生成 的,状态是别的的话 就是原来自带的)
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 任务名称(批量保存的时候用)
		/// </summary>
		public string Name {get;set;} 
		/// <summary>
		/// 工作内容(批量保存的时候用)
		/// </summary>
		public string Content { get;set;} 

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
		/// 父级id
		/// </summary>
		public Guid? ParentId {get;set;} 
		/// <summary>
		/// 是否历程碑 
		/// </summary>
		public bool IsMilestone { get; set; }
		/// <summary>
		/// 甘特图 item 编辑 标记
		/// </summary>
		public GanttItemState GanttItemState { get; set; }

		/// <summary>
		/// 前置任务 ids 
		/// </summary>
		public List<Guid> PreTaskIds { get; set; } = new List<Guid>();
		
		/// <summary>
		/// 计划任务id 
		/// </summary>
		public Guid? PlanId { get; set; }  

		/// <summary>
		/// 参考人工(多少人)
		/// </summary>
		public int WorkerNumber { get; set; } = 1;

	}
}