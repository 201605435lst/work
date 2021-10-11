using System;
using System.Collections.Generic;
using GenerateLibrary;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划工程量
	/// </summary>
	[Comment("施工计划工程量")]
	public class PlanMaterial : FullAuditedEntity<Guid>
	{
		protected PlanMaterial() { }
		public PlanMaterial(Guid id) => Id = id;

		/// <summary>
		/// 施工计划id 
		/// </summary>
		[Comment("施工计划id ")]
		[Display(DisplayType.Guid)]
		public Guid? PlanContentId { get; set; }
		/// <summary>
		/// 施工计划 
		/// </summary>
		[Comment("施工计划")]
		[Display(DisplayType.Entity)]
		public PlanContent PlanContent { get; set; }
		
		/// <summary>
		/// 设备列表
		/// </summary>
		[Comment("设备列表")]
		[Display(DisplayType.EntityList,nameof(PlanMaterialRltEquipment))]
		public List<PlanMaterialRltEquipment>  PlanMaterialRltEquipments { get; set; }

		/// <summary>
		/// 构件分类名称 
		/// </summary>
		public string ComponentCategoryName { get; set; }

		/// <summary>
		/// 规格型号(调用 TechnologyQuantityAppService.GetAllList().Where(x=>x.id===ComponentCategory.id))
		/// </summary>
		[Comment("规格型号")]
		[Display(DisplayType.String)]
		public string Spec { get; set; }
		

		/// <summary>
		/// 单位 (equipment.componentCategory.unit)
		/// </summary>
		[Comment("单位")]
		[Display(DisplayType.String)]
		public string Unit { get; set; }
		
		/// <summary>
		/// 数量 (设备列表的quantity 相加)
		/// </summary>
		[Comment("数量")]
		[Display(DisplayType.Decimal)]
		public decimal Quantity { get; set; }

		/// <summary>
		/// 工日
		/// </summary>
		[Comment("工日")]
		[Display(DisplayType.Int)]
		public decimal WorkDay { get; set; }

		public Guid? ProjectTagId { get; set; }
		public Guid? OrganizationRootTagId { get; set; }

	}
}