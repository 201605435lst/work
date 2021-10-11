using System;
using System.Collections.Generic;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 施工工序
	/// </summary>
	public class Procedure : FullAuditedEntity<Guid>
	{
		protected Procedure()
		{
		}

		public Procedure(Guid id)
		{
			Id = id;
		}


		/// <summary>
		/// 工序名称   
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 工序说明 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 工序类型 关联字典表
		/// </summary>
		public DataDictionary Type { get; set; }

		/// <summary>
		/// 工序类型 id   和 字典表 Progress.ProjectType 工程类型关联 
		/// </summary>
		public Guid TypeId { get; set; }

		/// <summary>
		/// 工序-工种 关联 
		/// </summary>
		public List<ProcedureWorker> ProcedureWorkers { get; set; }

		/// <summary>
		/// 工序-设备台班 关联 
		/// </summary>
		public List<ProcedureEquipmentTeam> ProcedureEquipmentTeams { get; set; }

		/// <summary>
		/// 工序-工程量清单 关联 
		/// </summary>
		public List<ProcedureMaterial> ProcedureMaterials { get; set; }

		/// <summary>
		/// 文件列表 
		/// </summary>
		public List<ProcedureRltFile> ProcedureRtlFiles { get; set; }
	}
}