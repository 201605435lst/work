using System;
using SnAbp.ConstructionBase.Enums;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分布分项-详情 Update dto
	/// </summary>
	public class SubItemContentUpdateDto
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