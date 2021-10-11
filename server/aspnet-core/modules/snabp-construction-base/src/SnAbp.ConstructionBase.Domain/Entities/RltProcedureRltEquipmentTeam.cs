using SnAbp.MultiProject.MultiProject;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.ConstructionBase.Entities
{
	/// <summary>
	/// 关联工序-设备台班 关联
	/// </summary>
	public class RltProcedureRltEquipmentTeam: Entity<Guid>
	{
		protected RltProcedureRltEquipmentTeam() { }
		public RltProcedureRltEquipmentTeam(Guid id ) => Id = id;
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
		public EquipmentTeam EquipmentTeam { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public int Count { get; set; }
	}
}