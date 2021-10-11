using System;
using SnAbp.Bpm.Dtos;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlanRltWorkflowInfo
{

	/// <summary>
	/// 施工计划审批记录 dto 
	/// </summary>
	public class MasterPlanRltWorkflowInfoDto : EntityDto<Guid>
	{

		/// <summary>
		/// 计划id
		/// </summary>
		public Guid? MasterPlanId {get;set;}
		/// <summary>
		/// 计划实体
		/// </summary>
		public MasterPlanDto MasterPlan {get;set;}
		/// <summary>
		/// 工作流id
		/// </summary>
		public Guid? WorkflowId {get;set;}
		/// <summary>
		/// 工作流实体
		/// </summary>
		public WorkflowDto Workflow {get;set;}
		/// <summary>
		/// 审批信息
		/// </summary>
		public string Content {get;set;}
		/// <summary>
		/// 审批状态
		/// </summary>
		public string WorkflowState {get;set;}
	}
}