using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.RltProcedure;
using SnAbp.ConstructionBase.Dtos.SubItem;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface ISubItemContentAppService
		: ICrudAppService<
			SubItemContentDto,
			Guid,
			SubItemContentSearchDto,
			SubItemContentCreateDto,
			SubItemContentUpdateDto
		>
	{
		/// <summary>
		/// 初始化一个 分布分项-详情 
		/// </summary>
		/// <param name="subItemId"></param>
		/// <returns></returns>
		Task<SubItemContentDto> InitContent(Guid subItemId);
		/// <summary>
		/// 根据 contentId 获取 单个 content 树
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		SubItemContentDto GetSingleTree(Guid id);

		/// <summary>
		/// 根据 subItemId 或取 单个 content 树
		/// </summary>
		/// <param name="subItemId"></param>
		/// <returns></returns>
		Task<SubItemContentDto> GetSingleTreeBySubItemId(Guid subItemId);

		/// <summary>
		/// 获取 单个 content 树 并且 在树上 续 关联 工序 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<SubItemContentDto> GetSingleTreeWithProcedure(Guid id);

		/// <summary>
		/// 根据 contentId 获取关联工序表 
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns></returns>
		Task<List<SubItemContentRltProcedureDto>> GetRltProceduresByContentId(Guid contentId);
		

		/// <summary>
		/// 给 content 设置 工序
		/// </summary>
		/// <param name="id"></param>
		/// <param name="procedureIds"></param>
		/// <returns></returns>
		Task<bool> AddProcedure(Guid id, List<Guid> procedureIds);
		
		
		
	}
}