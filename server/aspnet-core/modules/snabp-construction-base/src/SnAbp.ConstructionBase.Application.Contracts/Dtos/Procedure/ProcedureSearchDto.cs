using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Procedure
{
	/// <summary>
	/// 工序 搜索 param
	/// </summary>
	public class ProcedureSearchDto : PagedAndSortedResultRequestDto
	{
		/// <summary>
		/// 工序 id 查询 
		/// </summary>
		public Guid TypeId { get; set; }		/// <summary>
		/// 工序名称模糊搜索
		/// </summary>
		public string  Name { get; set; }
	}
}