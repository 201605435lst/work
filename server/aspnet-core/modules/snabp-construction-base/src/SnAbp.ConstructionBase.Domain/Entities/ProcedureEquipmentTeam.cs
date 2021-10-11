using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	public class ProcedureEquipmentTeam : Entity<Guid>
	{
		protected ProcedureEquipmentTeam()
		{
		}

		public ProcedureEquipmentTeam(Guid id ) => Id = id;

		/// <summary>
		/// 工序id 
		/// </summary>
		public Guid ProcedureId { get; set; }

		/// <summary>
		/// 设备台班 id  
		/// </summary>
		public Guid EquipmentTeamId { get; set; }
		/// <summary>
		/// 设备台班 实体  
		/// </summary>
		public EquipmentTeam EquipmentTeam { get; set; }
	}
}