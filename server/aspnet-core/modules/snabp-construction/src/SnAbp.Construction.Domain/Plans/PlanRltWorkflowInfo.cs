using System;
using GenerateLibrary;
using SnAbp.Bpm.Entities;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 计划审批记录
	/// </summary>
	[Comment("总体计划审批记录")]
	public class PlanRltWorkflowInfo : SingleFlowRltEntity
	{
		protected PlanRltWorkflowInfo() { }
		public PlanRltWorkflowInfo(Guid id) => Id = id;
		
		/// <summary>
		/// 施工计划id
		/// </summary>
		[Comment("施工计划id")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? PlanId { get; set; }
		/// <summary>
		/// 施工计划实体 
		/// </summary>
		[Comment("施工计划实体")]
		[Display(DisplayType.Entity)]
		public Plan Plan { get; set; }
	}
}