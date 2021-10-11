using System;
using System.Collections.Generic;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Dtos.Worker;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 工序 dto 
	/// </summary>
	public class ProcedureDto : EntityDto<Guid>
	{
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
		public string Type { get; set; }

		/// <summary>
		/// 工序类型 id 
		/// </summary>
		public Guid TypeId { get; set; }

		/// <summary>
		/// 工序-工种 关联 
		/// </summary>
		public List<ConstructionBaseWorkerDto> ProcedureWorkers { get; set; }

		/// <summary>
		/// 工序-设备台班 关联 
		/// </summary>
		public List<EquipmentTeamDto> ProcedureEquipmentTeams { get; set; }

		/// <summary>
		/// 工序-工程量清单 关联 
		/// </summary>
		public List<ProcedureMaterialDto> ProcedureMaterials { get; set; }

		/// <summary>
		/// 工序-工程量清单 关联 
		/// </summary>
		public List<ConstructionBaseFileDto> ProcedureRtlFiles { get; set; }
	}
}