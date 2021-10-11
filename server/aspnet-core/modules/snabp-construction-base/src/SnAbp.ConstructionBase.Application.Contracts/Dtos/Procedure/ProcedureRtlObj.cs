using System.Collections.Generic;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Dtos.Worker;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 工序 关联 对象  
	/// </summary>
	public class ProcedureRtlObj
	{
		public List<ConstructionBaseWorkerDto> Workers { get; set; }
		public List<EquipmentTeamDto> EquipmentTeams { get; set; }
		public List<ProcedureMaterialDto> Materials { get; set; }
	}
}