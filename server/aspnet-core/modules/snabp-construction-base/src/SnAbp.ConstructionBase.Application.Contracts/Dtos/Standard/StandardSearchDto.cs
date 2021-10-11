
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Standard 
{
	/// <summary>
	/// 工序规范维护 SearchDto (搜索Dto) 
	/// </summary>
	public class StandardSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{

		/// <summary>
		/// BlurSearch 根据Standard.Name模糊搜索,根据Standard.Code模糊搜索
		/// </summary>
		public string SearchKey {get;set;} 



		/// <summary>
		/// 根据 所属专业id 搜索
		/// </summary>
		public Guid? ProfessionId {get;set;} 

	}
}
