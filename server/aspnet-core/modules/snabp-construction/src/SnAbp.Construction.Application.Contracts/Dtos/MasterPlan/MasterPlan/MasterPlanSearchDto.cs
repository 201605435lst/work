using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos.MasterPlan.MasterPlan 
{
	/// <summary>
	/// 施工计划 SearchDto (搜索Dto) 
	/// </summary>
	public class MasterPlanSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{

		/// <summary>
		/// 根据 项目Id  搜索
		/// </summary>

		/// <summary>
		/// 模糊搜索 
		/// </summary>
		public string SearchKey { get; set; } 
		/// <summary>
		/// 是否查询审批数据（根据此字段过滤）
		/// </summary>
		public bool Approval { get; set; }
		/// <summary>
		/// 是否获取待我审批的数据
		/// </summary>
		public bool Waiting { get; set; }
		/// <summary>
		/// 是否只获取已审批的数据 
		/// </summary>
		public bool OnlyPass { get; set; }
                           
	}
}
