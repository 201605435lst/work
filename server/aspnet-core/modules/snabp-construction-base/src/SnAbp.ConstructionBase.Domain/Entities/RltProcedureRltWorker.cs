using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 关联工序-工种 关联
	/// </summary>
	public class RltProcedureRltWorker: Entity<Guid>
	{
		protected RltProcedureRltWorker() { }
		public RltProcedureRltWorker(Guid id ) => Id = id;
		
		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }
		
		/// <summary>
		/// 工种信息 id  
		/// </summary>
		public Guid WorkerId { get; set; }
		/// <summary>
		/// 工种信息 实体  
		/// </summary>
		public Worker Worker { get; set; }
		/// <summary>
		/// 数量(工日)
		/// </summary>
		public int Count { get; set; }
	}
}