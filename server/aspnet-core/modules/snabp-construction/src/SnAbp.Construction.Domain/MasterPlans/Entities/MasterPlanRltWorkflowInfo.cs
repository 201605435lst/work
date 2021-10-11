using System;
using GenerateLibrary;
using SnAbp.Bpm;
using SnAbp.Bpm.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.MasterPlans.Entities
{
	/// <summary>
	/// 总体计划审批记录
	/// </summary>
	[Comment("总体计划审批记录")]
	public class MasterPlanRltWorkflowInfo :  FullAuditedEntity<Guid>
	{
		protected MasterPlanRltWorkflowInfo() { }
		public MasterPlanRltWorkflowInfo(Guid id) => Id = id;
		
		/// <summary>
		/// 计划id
		/// </summary>
		[Comment("计划id")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? MasterPlanId { get; set; }
		/// <summary>
		/// 计划实体 
		/// </summary>
		[Comment("计划实体")]
		[Display(DisplayType.Entity)]
		public MasterPlan MasterPlan { get; set; }
		/// <summary>
		/// 工作流id
		/// </summary>
		[Comment("工作流id")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? WorkflowId { get; set; }
		/// <summary>
		/// 工作流实体 
		/// </summary>
		[Comment("工作流实体")]
		[Display(DisplayType.Entity)]
		public Workflow Workflow { get; set; }

		/// <summary>
		/// 审批信息 
		/// </summary>
		[Comment("审批信息")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Content { get; set; }
		/// <summary>
		/// 审批状态 
		/// </summary>
		[Comment("审批状态")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.EnumSelect)]
		[Display(DisplayType.String)]
		public WorkflowState WorkflowState { get; set; }
	}
}