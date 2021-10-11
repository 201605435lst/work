using System;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.ConstructionBase.Dtos.Material;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.Plan.PlanContentRltMaterial
{

	/// <summary>
	/// 未写注释 dto 
	/// </summary>
	public class PlanContentRltMaterialDto : EntityDto<Guid>
	{

		/// <summary>
		/// 施工计划详情id 
		/// </summary>
		public Guid? PlanContentId {get;set;}
		/// <summary>
		/// 施工计划详情实体
		/// </summary>
		public PlanContentDto PlanContent {get;set;}
		/// <summary>
		/// 工程量实体
		/// </summary>
		public ConstructionBaseMaterialDto Material {get;set;}
		/// <summary>
		/// 工程量id
		/// </summary>
		public Guid? MaterialId {get;set;}
		/// <summary>
		/// 数量
		/// </summary>
		public int Count {get;set;}
	}
}