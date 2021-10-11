using System;
using System.Collections.Generic;
using SnAbp.ConstructionBase.Dtos.Procedure;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.RltProcedure
{
	/// <summary>
	/// 工序 - Content dto 
	/// </summary>
	public class SubItemContentRltProcedureDto : EntityDto<Guid>
	{
		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }
		/// <summary>
		/// 工序 实体 
		/// </summary>
		public ProcedureDto Procedure { get; set; }

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
		public List<RltProcedureRltWorkerDto> ProcedureWorkers { get; set; }
		/// <summary>
		/// 关联工序-设备台班 关联 
		/// </summary>
		public List<RltProcedureRltEquipmentTeamDto> ProcedureEquipmentTeams { get; set; }
		/// <summary>
		/// 关联工序-工程量清单 关联 
		/// </summary>
		public List<RltProcedureRltMaterialDto> ProcedureMaterials { get; set; }
		/// <summary>
		/// 关联工序-文件列表 关联 
		/// </summary>
		public List<RltProcedureRltFileDto> ProcedureRtlFiles { get; set; }
	}
}