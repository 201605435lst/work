
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.PlanMaterial 
{
	/// <summary>
	/// 施工计划工程量 SearchDto (搜索Dto) 
	/// </summary>
	public class PlanMaterialSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{
		/// <summary>
		/// 根据 施工计划详情id 查询 
		/// </summary>
		public Guid? PlanContentId { get; set; }
		

	}
}
