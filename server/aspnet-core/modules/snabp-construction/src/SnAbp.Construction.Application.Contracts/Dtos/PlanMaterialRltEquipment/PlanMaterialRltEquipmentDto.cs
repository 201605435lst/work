
using System;
using System.Collections.Generic;
using SnAbp.Construction.Dtos.PlanMaterial;
using SnAbp.Resource.Dtos;
using Volo.Abp.Application.Dtos;
namespace SnAbp.Construction.Dtos.PlanMaterialRltEquipment
{

	/// <summary>
	/// 施工计划工程量 关联 设备  dto 
	/// </summary>
	public class PlanMaterialRltEquipmentDto : EntityDto<Guid>
	{

		/// <summary>
		/// 设备实体
		/// </summary>
		public EquipmentDto Equipment {get;set;}
		/// <summary>
		/// 施工计划工程量
		/// </summary>
		public PlanMaterialDto PlanMaterial {get;set;}
	}
}