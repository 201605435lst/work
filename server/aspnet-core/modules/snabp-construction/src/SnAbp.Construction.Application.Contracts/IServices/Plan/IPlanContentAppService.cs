using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices.Plan
{
	/// <summary>
	/// 施工计划详情 IService 接口
	/// </summary>
	public interface IPlanContentAppService : ICrudAppService< 
		PlanContentDto,
		Guid,
		PlanContentSearchDto,
		PlanContentCreateDto,
		PlanContentUpdateDto
	>
	{
		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);

		/// <summary>
		/// 根据 planId 获取年份
		/// </summary>
		/// <returns></returns>
		Task<List<int>> GetYears(Guid planId);
		
	}
}
