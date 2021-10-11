using System;
using System.Collections.Generic;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanRltContentRltAntecedent;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent
{
	/// <summary>
	/// 施工计划详情 dto 
	/// </summary>
	public class MasterPlanContentDto : EntityDto<Guid>,IPlanDto<MasterPlanContentDto>
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
		/// 施工总体计划Id 
		/// </summary>
		public Guid? MasterPlanId {get;set;}
		/// <summary>
		/// 施工总体计划
		/// </summary>
		public MasterPlanDto MasterPlan {get;set;}
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
		/// 父级id
		/// </summary>
		public Guid? ParentId {get;set;}
		/// <summary>
		/// 父级
		/// </summary>
		public MasterPlanContentDto Parent {get;set;}

		/// <summary>
		/// 子级列表
		/// </summary>
		public List<MasterPlanContentDto> Children { get; set; } = new List<MasterPlanContentDto>();
		
		/// <summary>
		/// 是否里程碑
		/// </summary>
		public bool IsMilestone { get; set; }

		/// <summary>
		/// 前置任务
		/// </summary>
		public List<MasterPlanRltContentRltAntecedentDto> Antecedents { get; set; } = new List<MasterPlanRltContentRltAntecedentDto>();

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}