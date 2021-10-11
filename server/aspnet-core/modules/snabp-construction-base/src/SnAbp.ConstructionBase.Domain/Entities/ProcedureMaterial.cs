using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	public class ProcedureMaterial : Entity<Guid>
	{
		protected ProcedureMaterial()
		{
		}

		public ProcedureMaterial(Guid id ) => Id = id;

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }

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