using System;
using System.Collections.Generic;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.Plan;
using SnAbp.Construction.Dtos.Plan.PlanContentRltAntecedent;
using SnAbp.Construction.Dtos.Plan.PlanContentRltFile;
using SnAbp.Construction.Dtos.Plan.PlanContentRltMaterial;
using SnAbp.Construction.Dtos.PlanMaterial;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.PlanContent
{

	/// <summary>
	/// 施工计划详情 dto 
	/// </summary>
	public class PlanContentDto : EntityDto<Guid> ,IPlanDto<PlanContentDto>
	{

		/// <summary>
		/// 分部分项Id 
		/// </summary>
		public Guid? SubItemContentId {get;set;}
		/// <summary>
		/// 分部分项
		/// </summary>
		public SubItemContentDto SubItemContent {get;set;}
		/// <summary>
		/// 施工计划Id 
		/// </summary>
		public Guid? PlanId {get;set;}
		/// <summary>
		/// 施工计划
		/// </summary>
		public PlanDto Plan {get;set;}
		/// <summary>
		/// 计划名称
		/// </summary>
		public string Name {get;set;}
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
		/// 是否历程碑
		/// </summary>
		public bool IsMilestone {get;set;}
		/// <summary>
		/// 父级id
		/// </summary>
		public Guid? ParentId {get;set;}
		/// <summary>
		/// 父e
		/// </summary>
		public PlanContentDto Parent {get;set;}
		/// <summary>
		/// 子级列表
		/// </summary>
		public List<PlanContentDto> Children {get;set;} = new List<PlanContentDto>(); // 数组类型最好给初始值,不然容易报null的错
		/// <summary>
		/// 前置任务
		/// </summary>
		public List<PlanContentRltAntecedentDto> Antecedents {get;set;} = new List<PlanContentRltAntecedentDto>(); // 数组类型最好给初始值,不然容易报null的错
		/// <summary>
		/// 相关资料
		/// </summary>
		public List<PlanContentRltFileDto> Files {get;set;} = new List<PlanContentRltFileDto>(); // 数组类型最好给初始值,不然容易报null的错
		/// <summary>
		/// 整体进度
		/// </summary>
		public double AllProgress {get;set;}
		
		/// <summary>
		/// 施工计划工程量
		/// </summary>
		public List<PlanMaterialDto> PlanMaterials { get; set; }
		/// <summary>
		/// 工日
		/// </summary>
		public decimal WorkDay { get; set; } 
		/// <summary>
		/// 参考人工(多少人)
		/// </summary>
		public int WorkerNumber { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}