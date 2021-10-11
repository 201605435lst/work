using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.EquipmentTeam
{
	public class EquipmentTeamSearchDto : PagedAndSortedResultRequestDto
	{
		/// <summary>
		/// 设备名称 (BlurSearch)
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 设备类型id 
		/// </summary>
		public Guid TypeId { get; set; }
	}
}