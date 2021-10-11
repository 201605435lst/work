
using System;
using System.Collections.Generic;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.Construction.Dtos.PlanMaterialRltEquipment;
using Volo.Abp.Application.Dtos;
namespace SnAbp.Construction.Dtos.PlanMaterial
{

	/// <summary>
	/// 施工计划工程量 dto 
	/// </summary>
	public class PlanMaterialDto : EntityDto<Guid>
	{

		/// <summary>
		/// 施工计划id 
		/// </summary>
		public Guid? PlanContentId {get;set;}
		/// <summary>
		/// 施工计划
		/// </summary>
		public PlanContentDto PlanContent {get;set;}
		/// <summary>
		/// 设备列表
		/// </summary>
		public List<PlanMaterialRltEquipmentDto> PlanMaterialRltEquipments {get;set;} = new List<PlanMaterialRltEquipmentDto>(); // 数组类型最好给初始值,不然容易报null的错
		/// <summary>
		/// 规格型号
		/// </summary>
		public string Spec {get;set;}
		/// <summary>
		/// 单位
		/// </summary>
		public string Unit {get;set;}
		/// <summary>
		/// 数量
		/// </summary>
		public double Quantity {get;set;}
		/// <summary>
		/// 工日
		/// </summary>
		public decimal WorkDay {get;set;}
		/// <summary>
		/// 构件分类名称 
		/// </summary>
		public string ComponentCategoryName { get; set; }
	}
}