using System;

namespace SnAbp.ConstructionBase.Dtos.EquipmentTeam
{
	/// <summary>
	/// 添加设备台班 dto 
	/// </summary>
	public class EquipmentTeamCreateDto
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
		/// 设备类型 id 
		/// </summary>
		public Guid TypeId { get; set; }
	}
}