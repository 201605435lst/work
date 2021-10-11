using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanRltContentRltAntecedent
{

	/// <summary>
	/// 施工总体计划任务前置表 dto 
	/// </summary>
	public class MasterPlanRltContentRltAntecedentDto : EntityDto<Guid>
	{

		/// <summary>
		/// 前置计划详情id 
		/// </summary>
		public Guid? MasterPlanContentId {get;set;}
		
		public Guid? FrontMasterPlanContentId { get; set; }
		
		/// <summary>
		/// 关联 详情  id 
		/// </summary>
		public Guid? RltId { get; set; }
		
		/// <summary>
		/// 计划详情- 名称  
		/// </summary>
		public string Name {get;set;}
		
	}
}