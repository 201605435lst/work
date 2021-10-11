using System;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分布分项 -更新 -dto
	/// </summary>
	public class SubItemUpdateDto
	{
		/// <summary>
		/// 工程名称   
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 备注    
		/// </summary>
		public string Remarks { get; set; }
	}
}