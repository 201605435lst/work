using System;
using System.Collections.Generic;
using GenerateLibrary;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.ConstructionBase.Entities;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划详情
	/// </summary>
	[Comment("施工计划详情")]
	public class PlanContent :  FullAuditedEntity<Guid>, IPlanEntity<PlanContent>
	{
		protected PlanContent() { }
		public PlanContent(Guid id) => Id = id;
		public Guid? ProjectTagId { get; set; }
		public Guid? OrganizationRootTagId { get; set; }
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
		/// 施工计划id
		/// </summary>
		[Comment("施工计划Id ")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? PlanId { get; set; }
		/// <summary>
		/// 施工计划 
		/// </summary>
		[Comment("施工计划")]
		[Display(DisplayType.Entity)]
		public Plan Plan { get; set; }

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
		[Create(CreateType.BoolSwitch)]
		[Display(DisplayType.Bool)]
		public bool IsMilestone { get; set; }

		[Comment("父级id")]
		[Create(CreateType.GuidSelect)]
		[Display(DisplayType.Guid)]
		public Guid? ParentId { get; set; }
		[Comment("父级")]
		[Display(DisplayType.EntityParent)]
		public PlanContent Parent { get; set; }
		[Comment("子级列表")]
		[Display(DisplayType.EntityChildrenList)]
		public List<PlanContent> Children { get; set; }
		
		/// <summary>
		/// 前置任务
		/// </summary>
		[Comment("前置任务")]
		[Display(DisplayType.EntityList,nameof(PlanContentRltAntecedent))]
		public List<PlanContentRltAntecedent> Antecedents { get; set; }

		/// <summary>
		/// 相关资料 
		/// </summary>
		[Comment("相关资料")]
		[Display(DisplayType.EntityList,nameof(PlanContentRltFile))]
		public List<PlanContentRltFile> Files { get; set; } = new List<PlanContentRltFile>();
		
		/// <summary>
		/// 整体进度 
		/// </summary>
		[Comment("整体进度")]
		[Display(DisplayType.Decimal)]
		public double AllProgress { get; set; }

		/// <summary>
		/// 施工计划工程量
		/// </summary>
		[Comment("施工计划工程量")]
		[Display(DisplayType.EntityList,nameof(PlanMaterial))]
		public List<PlanMaterial> PlanMaterials { get; set; }

		/// <summary>
		/// 工日
		/// </summary>
		[Comment("工日")]
		public decimal WorkDay { get; set; }

		/// <summary>
		/// 参考人工(多少人)
		/// </summary>
		[Comment("参考人工")]
		public int WorkerNumber { get; set; } = 1;

	}
}