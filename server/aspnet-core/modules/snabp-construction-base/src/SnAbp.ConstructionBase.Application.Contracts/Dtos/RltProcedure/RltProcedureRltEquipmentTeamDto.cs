using System;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.RltProcedure
{
	public class RltProcedureRltEquipmentTeamDto : EntityDto<Guid>
	{
		/// <summary>
		/// 关联工序id 
		/// </summary>
		public Guid RltProcedureId { get; set; }
		/// <summary>
		/// 设备台班 id  
		/// </summary>
		public Guid EquipmentTeamId { get; set; }
		/// <summary>
		/// 设备台班 实体  
		/// </summary>
		public EquipmentTeamDto EquipmentTeam { get; set; }
	}
}