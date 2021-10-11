using System;
using SnAbp.Construction.Dtos.Plan.Plan;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.PlanRltWorkflowInfo
{

	/// <summary>
	/// 总体计划审批记录 dto 
	/// </summary>
	public class PlanRltWorkflowInfoDto : EntityDto<Guid>
	{

		/// <summary>
		/// 施工计划id
		/// </summary>
		public Guid? PlanId {get;set;}
		/// <summary>
		/// 施工计划实体
		/// </summary>
		public PlanDto Plan {get;set;}
	}
}