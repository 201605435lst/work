using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Worker
{
	/// <summary>
	/// worker 查询 parameter
	/// </summary>
	public class WorkerSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{
		//  工种名称 
		public string Name { get; set; }
	}
	
}