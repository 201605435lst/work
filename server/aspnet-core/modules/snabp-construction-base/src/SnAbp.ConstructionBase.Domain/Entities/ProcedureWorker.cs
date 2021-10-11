using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	public class ProcedureWorker : Entity<Guid>
	{
		protected ProcedureWorker() { }
		public ProcedureWorker(Guid id ) => Id = id;

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }

		/// <summary>
		/// 工种信息 id  
		/// </summary>
		public Guid WorkerId { get; set; }
		/// <summary>
		/// 工种信息 实体  
		/// </summary>
		public Worker Worker { get; set; }
	}
}