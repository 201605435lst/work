using System;
using GenerateLibrary;
using SnAbp.Resource.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Construction.Plans
{
	/// <summary>
	/// 施工计划工程量 关联 设备 
	/// </summary>
	[Comment("施工计划工程量 关联 设备 ")]
	public class PlanMaterialRltEquipment : FullAuditedEntity<Guid>
	{
		protected PlanMaterialRltEquipment() { }
		public PlanMaterialRltEquipment(Guid id) => Id = id;
		/// <summary>
		/// 设备实体
		/// </summary>
		[Comment("设备实体")]
		[Display(DisplayType.Entity)]
		public Equipment  Equipment { get; set; }

		/// <summary>
		/// 设备实体id
		/// </summary>
		[Comment("设备实体id")]
		[Display(DisplayType.Guid)]
		public Guid EquipmentId { get; set; }

		/// <summary>
		/// 施工计划工程量
		/// </summary>
		[Comment("施工计划工程量")]
		[Display(DisplayType.Entity)]
		public PlanMaterial  PlanMaterial { get; set; }
		/// <summary>
		/// 施工计划工程量id
		/// </summary>
		[Comment("施工计划工程量id")]
		[Display(DisplayType.Guid)]
		public Guid PlanMaterialId { get; set; }
		
	}
}