using System;
using GenerateLibrary;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划任务前置表
	/// </summary>
	[Comment("施工计划任务前置表")]
	public class PlanContentRltAntecedent: FullAuditedEntity<Guid>
	{
		protected PlanContentRltAntecedent() { }
		public PlanContentRltAntecedent(Guid id) => Id = id;
		
		/// <summary>
		/// 前置计划详情id 
		/// </summary>
		[Comment("前置计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? FrontPlanContentId { get; set; }

		/// <summary>
		/// 前置计划详情实体 
		/// </summary>
		[Comment("前置计划详情实体")]
		[Display(DisplayType.Entity)]
		public PlanContent FrontPlanContent { get; set; }

		/// <summary>
		/// 计划详情id 
		/// </summary>
		[Comment("计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? PlanContentId { get; set; }

		/// <summary>
		/// 计划详情实体 
		/// </summary>
		[Comment("计划详情实体")]
		[Display(DisplayType.Entity)]
		public PlanContent PlanContent { get; set; }

	}
}