using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 关联工序-工程量 关联
	/// </summary>
	public class RltProcedureRltMaterial: Entity<Guid>
	{
		protected RltProcedureRltMaterial() { }
		public RltProcedureRltMaterial(Guid id ) => Id = id;
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
		public ConstructionBaseMaterial ConstructionBaseMaterial { get; set; }
		/// <summary>
		/// 数量
		/// </summary>
		public int Count { get; set; }
	}
}