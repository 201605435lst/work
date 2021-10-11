
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Section 
{
	/// <summary>
	/// 未写注释 SearchDto (搜索Dto) 
	/// </summary>
	public class SectionSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{

		/// <summary>
		/// BlurSearch 根据Section.Name模糊搜索,根据Section.Desc模糊搜索
		/// </summary>
		public string SearchKey {get;set;} 



	}
}
