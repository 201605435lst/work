using System;
using GenerateLibrary;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.MasterPlans.Entities
{
	/// <summary>
	/// 总体计划任务前置表
	/// </summary>
	[Comment("总体计划任务前置表")]
	public class MasterPlanRltContentRltAntecedent: FullAuditedEntity<Guid>
	{
		protected MasterPlanRltContentRltAntecedent() { }
		public MasterPlanRltContentRltAntecedent(Guid id) => Id = id;
		
		/// <summary>
		/// 前置计划详情id 
		/// </summary>
		[Comment("前置计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? FrontMasterPlanContentId { get; set; }

		/// <summary>
		/// 前置计划详情实体 
		/// </summary>
		[Comment("前置计划详情实体")]
		[Display(DisplayType.Entity)]
		public MasterPlanContent FrontMasterPlanContent { get; set; }

		/// <summary>
		/// 计划详情id 
		/// </summary>
		[Comment("计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? MasterPlanContentId { get; set; }

		/// <summary>
		/// 计划详情实体 
		/// </summary>
		[Comment("计划详情实体")]
		[Display(DisplayType.Entity)]
		public MasterPlanContent MasterPlanContent { get; set; }

	}
}