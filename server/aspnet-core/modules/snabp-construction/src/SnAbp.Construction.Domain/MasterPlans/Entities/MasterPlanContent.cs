using System;
using System.Collections.Generic;
using GenerateLibrary;
using SnAbp.ConstructionBase.Entities;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.MasterPlans.Entities
{
	/// <summary>
	/// 总体计划详情
	/// </summary>
	[Comment("总体计划详情")]
	public class MasterPlanContent :  FullAuditedEntity<Guid>,IPlanEntity<MasterPlanContent>
	{
		protected MasterPlanContent() { }
		public MasterPlanContent(Guid id) => Id = id;
		
		/// <summary>
		/// 分部分项id
		/// </summary>
		[Comment("分部分项Id ")]
		[Display(DisplayType.Guid)]
		public Guid? SubItemContentId { get; set; }
		/// <summary>
		/// 分部分项 
		/// </summary>
		[Comment("分部分项")]
		[Display(DisplayType.Entity)]
		public SubItemContent SubItemContent { get; set; }
		/// <summary>
		/// 总体总体计划id
		/// </summary>
		[Comment("总体计划Id ")]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.Guid)]
		public Guid? MasterPlanId { get; set; }
		/// <summary>
		/// 总体总体计划 
		/// </summary>
		[Comment("总体计划")]
		[Display(DisplayType.Entity)]
		public MasterPlan MasterPlan { get; set; }

		/// <summary>
		/// 计划名称 
		/// </summary>
		[Comment("计划名称")]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Name { get; set; }
		/// <summary>
		/// 计划描述 
		/// </summary>
		[Comment("计划描述")]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Content { get; set; }
		/// <summary>
		/// 计划开始时间 
		/// </summary>
		[Comment("计划开始时间")]
		[Create(CreateType.DatePicker)]
		[Display(DisplayType.DateTime)]
		public DateTime PlanStartTime { get; set; }
		/// <summary>
		/// 计划结束时间 
		/// </summary>
		[Comment("计划结束时间")]
		[Create(CreateType.DatePicker)]
		[Display(DisplayType.DateTime)]
		public DateTime PlanEndTime { get; set; }
		/// <summary>
		/// 工期 
		/// </summary>
		[Comment("工期")]
		[Create(CreateType.DecimalInput)]
		[Display(DisplayType.Decimal)]
		public double Period { get; set; }
		/// <summary>
		/// 是否历程碑 
		/// </summary>
		[Comment("是否历程碑")]
		[Create(CreateType.DecimalInput)]
		[Display(DisplayType.Decimal)]
		public bool IsMilestone { get; set; }

		[Comment("父级id")]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.Guid)]
		public Guid? ParentId { get; set; }
		[Comment("父级")]
		[Display(DisplayType.EntityParent)]
		public MasterPlanContent Parent { get; set; }
		[Comment("子级列表")]
		[Display(DisplayType.EntityChildrenList)]
		public List<MasterPlanContent> Children { get; set; }
		
		/// <summary>
		/// 前置任务
		/// </summary>
		[Comment("前置任务")]
		[Display(DisplayType.EntityList,nameof(MasterPlanRltContentRltAntecedent))]
		public List<MasterPlanRltContentRltAntecedent> Antecedents { get; set; }
	}
}