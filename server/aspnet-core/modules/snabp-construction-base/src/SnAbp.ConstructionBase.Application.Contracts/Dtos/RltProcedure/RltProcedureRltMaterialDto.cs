using System;
using SnAbp.ConstructionBase.Dtos.Material;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.RltProcedure
{
	public class RltProcedureRltMaterialDto : EntityDto<Guid>
	{
		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }
		/// <summary>
		/// 工程量清单 id  
		/// </summary>
		public Guid MaterialId { get; set; }
		/// <summary>
		/// 工程量清单 实体  
		/// </summary>
		public ConstructionBaseMaterialDto Material { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public int Count { get; set; }
	}
}