using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.EquipmentTeam
{
	public class EquipmentTeamDto : EntityDto<Guid>
	{
		/// <summary>
		/// 设备名称 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 设备规格 
		/// </summary>
		public string Spec { get; set; }

		/// <summary>
		/// 设备成本
		/// </summary>
		public double Cost { get; set; }

		/// <summary>
		/// 设备类型 
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 设备类型 id 
		/// </summary>
		public Guid TypeId { get; set; }

		public int Count { get; set; }
	}
}