using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 关联工序-文件 关联
	/// </summary>
	public class RltProcedureRltFile: Entity<Guid>
	{
		protected RltProcedureRltFile() { }
		public RltProcedureRltFile(Guid id ) => Id = id;
		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }
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