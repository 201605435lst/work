using System;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.PlanContentRltAntecedent
{

	/// <summary>
	/// 总体计划任务前置表 dto 
	/// </summary>
	public class PlanContentRltAntecedentDto : EntityDto<Guid>
	{

		/// <summary>
		/// 前置计划详情id 
		/// </summary>
		public Guid? FrontPlanContentId {get;set;}
		/// <summary>
		/// 前置计划详情实体
		/// </summary>
		public PlanContentDto FrontPlanContent {get;set;}
		/// <summary>
		/// 计划详情id 
		/// </summary>
		public Guid? PlanContentId {get;set;}
		/// <summary>
		/// 计划详情实体
		/// </summary>
		public PlanContentDto PlanContent {get;set;}
	}
}