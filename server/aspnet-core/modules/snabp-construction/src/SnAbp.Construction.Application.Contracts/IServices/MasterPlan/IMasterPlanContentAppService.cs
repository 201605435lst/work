using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices.MasterPlan
{
	/// <summary>
	/// 施工计划详情 IService 接口
	/// </summary>
	public interface IMasterPlanContentAppService : ICrudAppService< 
		MasterPlanContentDto,
		Guid,
		MasterPlanContentSearchDto,
		MasterPlanContentCreateDto,
		MasterPlanContentUpdateDto
	>
	{
		/// <summary>
		/// 根据 施工计划id 获取 详情树(单树)
		/// </summary>
		/// <param name="masterPlanId"></param>
		/// <returns></returns>
		List<MasterPlanContentDto> GetSingleTree(Guid masterPlanId,PlanContentDateFilterDto filter);

		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);
		
	}
}
