using System;
using System.Collections.Generic;
using GenerateLibrary;
using SnAbp.Bpm.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划
	/// </summary>
	[Comment("施工计划")]
	public class Plan : SingleFlowEntity
	{
		protected Plan() { }
		public Plan(Guid id) => Id = id;
		/// <summary>
		/// 项目id
		/// </summary>
		[Comment("项目Id ")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		[Search(SearchType.IdSearch)]
		public Guid? ProjectTagId { get; set; }
		public Guid? OrganizationRootTagId { get; set; }

		/// <summary>
		/// 总体计划id
		/// </summary>
		[Comment("总体计划Id ")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		[Search(SearchType.IdSearch)]
		public Guid? MasterPlanId { get; set; }
		/// <summary>
		/// 总体计划 
		/// </summary>
		[Comment("总体计划")]
		[Display(DisplayType.Entity)]
		public MasterPlan MasterPlan { get; set; }

		/// <summary>
		/// 计划名称 
		/// </summary>
		[Comment("计划名称")]
		[Search(SearchType.BlurSearch)]
		[Create(CreateType.StringInput)]
		[Display(DisplayType.String)]
		public string Name { get; set; }
		/// <summary>
		/// 计划描述 
		/// </summary>
		[Comment("计划描述")]
		[Search(SearchType.BlurSearch)]
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
		/// 计划工期 
		/// </summary>
		[Comment("计划工期")]
		[Create(CreateType.DecimalInput)]
		[Display(DisplayType.Decimal)]
		public double Period { get; set; }
		/// <summary>
		/// 负责人 
		/// </summary>
		[Comment("负责人")]
		[Display(DisplayType.Entity)]
		public IdentityUser Charger { get; set; }
		/// <summary>
		/// 负责人Id 
		/// </summary>
		[Comment("负责人Id ")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? ChargerId { get; set; }

		/// <summary>
		/// 施工计划审批记录
		/// </summary>
		[Comment("施工计划审批记录")]
		[Display(DisplayType.EntityList,nameof(PlanRltWorkflowInfo))]
		public List<PlanRltWorkflowInfo> PlanRltWorkflowInfos { get; set; } = new List<PlanRltWorkflowInfo>();

		/// <summary>
		/// 施工计划详情实体 
		/// </summary>
		[Comment("施工计划详情实体")]
		[Display(DisplayType.Entity)]
		public PlanContent PlanContent { get; set; }

		/// <summary>
		/// 审批流程状态 默认未提交 
		/// </summary>
		[Comment("审批流程状态")]
		[Display(DisplayType.String)]
		[Create(CreateType.EnumSelect)]
		public MasterPlanState State { get; set; } = MasterPlanState.ToSubmit;




	}
}