using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 工序关联 
	/// </summary>
	public class SubItemContentRltProcedure : Entity<Guid>
	{
		protected SubItemContentRltProcedure() { }
		public SubItemContentRltProcedure(Guid id) => Id = id;

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }
		/// <summary>
		/// 工序 实体 
		/// </summary>
		public Procedure Procedure { get; set; }

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid SubItemContentId { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public int Sort { get; set; }
		
		/// <summary>
		/// 关联工序-工种 关联 
		/// </summary>
		public List<RltProcedureRltWorker> ProcedureWorkers { get; set; }
		/// <summary>
		/// 关联工序-设备台班 关联 
		/// </summary>
		public List<RltProcedureRltEquipmentTeam> ProcedureEquipmentTeams { get; set; }
		/// <summary>
		/// 关联工序-工程量清单 关联 
		/// </summary>
		public List<RltProcedureRltMaterial> ProcedureMaterials { get; set; }
		/// <summary>
		/// 关联工序-文件列表 关联 
		/// </summary>
		public List<RltProcedureRltFile> ProcedureRtlFiles { get; set; }

	}
}