using System;
using System.Collections.Generic;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 资源配置  工序 dto 
	/// </summary>
	public class ProcedureConfigDto
	{
		/// <summary>
		/// 工种id 
		/// </summary>
		public List<Guid> WorkerIds { get; set; }

		/// <summary>
		/// 工程量 {id ,count } 
		/// </summary>
		public List<MaterialConfigDto> MaterialIds { get; set; }

		/// <summary>
		/// 设备台班id 
		/// </summary>
		public List<Guid> EquipmentTeamIds { get; set; }

		/// <summary>
		/// 文件列表id 
		/// </summary>
		public List<Guid> FileIds { get; set; }
	}
}