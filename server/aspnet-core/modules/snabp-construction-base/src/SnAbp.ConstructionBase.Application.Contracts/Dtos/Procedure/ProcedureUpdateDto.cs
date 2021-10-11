using System;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 编辑  工序 dto 
	/// </summary>
	public class ProcedureUpdateDto
	{
		/// <summary>
		/// 工序名称   
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 工序说明 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 工序类型 id 
		/// </summary>
		public Guid TypeId { get; set; }
	}
}