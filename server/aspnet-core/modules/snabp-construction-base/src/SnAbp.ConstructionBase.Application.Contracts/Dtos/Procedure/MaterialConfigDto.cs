using System;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 工程量 config dto 
	/// </summary>
	public class MaterialConfigDto
	{
		/// <summary>
		/// 工程量 id 
		/// </summary>
		public Guid MaterialId { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public int Count { get; set; }
	}
}