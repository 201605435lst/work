using System;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 工序 - 工程量 dto 
	/// </summary>
	public class ProcedureMaterialDto
	{
		/// <summary>
		/// 工程量 id 
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 计量单位 转换为汉字
		/// </summary>
		public string UnitStr { get; set; }

		/// <summary>
		/// 工程量 名称  
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 工程量 数量
		/// </summary>
		public int Count { get; set; }
	}
}