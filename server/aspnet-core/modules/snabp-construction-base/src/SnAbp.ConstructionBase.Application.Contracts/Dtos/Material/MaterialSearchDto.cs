using Volo.Abp.Application.Dtos;

namespace SnAbp.ConstructionBase.Dtos.Material
{
	/// <summary>
	/// 工程量清单 查询 parameter
	/// </summary>
	public class MaterialSearchDto : PagedAndSortedResultRequestDto //PagedAndSortedResultRequestDto具有标准分页和排序属性
	{
		/// <summary>
		///  名称及规格 模糊查询
		/// </summary>
		public string Name { get; set; }
	}
}