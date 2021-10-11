using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	public class ProcedureRltFile : Entity<Guid>
	{
		protected ProcedureRltFile() { }
		public ProcedureRltFile(Guid id) => Id = id;

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }

		/// <summary>
		/// 工程量清单 id  
		/// </summary>
		public Guid FileId { get; set; }
		/// <summary>
		/// 工程量清单 实体  
		/// </summary>
		public virtual File.Entities.File File { get; set; }
	}
}