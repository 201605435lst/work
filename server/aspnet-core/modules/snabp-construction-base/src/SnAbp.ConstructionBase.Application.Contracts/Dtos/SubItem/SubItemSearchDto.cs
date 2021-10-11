using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.SubItem
{
	/// <summary>
	/// 分布分项 -搜索 -dto (parameter)
	/// </summary>
	public class SubItemSearchDto : PagedAndSortedResultRequestDto
	{
		/// <summary>
		/// 根据 项目 id 搜索 
		/// </summary>
		public Guid ProjectId { get; set; }
	}
}