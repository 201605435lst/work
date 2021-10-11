using System;
using GenerateLibrary;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划关联工程量清单
	/// </summary>
	public class PlanContentRltMaterial: FullAuditedEntity<Guid>
	{
		protected PlanContentRltMaterial()
		{
		}

		public PlanContentRltMaterial(Guid id) => Id = id;

		/// <summary>
		/// 施工计划详情id 
		/// </summary>
		[Comment("施工计划详情id ")]
		[Display(DisplayType.Guid)]
		public Guid? PlanContentId { get; set; }

		/// <summary>
		/// 施工计划详情实体 
		/// </summary>
		[Comment("施工计划详情实体")]
		[Display(DisplayType.Entity)]
		public PlanContent PlanContent { get; set; }

		///// <summary>
		///// 工程量实体 
		///// </summary>
		//[Comment("工程量实体")]
		//[Display(DisplayType.Entity)]
		//public Material Material { get; set; }

		///// <summary>
		///// 工程量id 
		///// </summary>
		//[Comment("工程量id")]
		//[Display(DisplayType.Guid)]
		//public Guid? MaterialId { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		[Comment("数量")]
		[Display(DisplayType.Int)]
		public int Count { get; set; }

	}
}