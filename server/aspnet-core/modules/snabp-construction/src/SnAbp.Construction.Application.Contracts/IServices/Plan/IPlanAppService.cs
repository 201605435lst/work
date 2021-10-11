using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.Plan.Plan;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices.Plan
{
	/// <summary>
	/// 施工计划 IService 接口
	/// </summary>
	public interface IPlanAppService : ICrudAppService< 
		PlanDto,
		Guid,
		PlanSearchDto,
		PlanCreateDto,
		PlanUpdateDto
	>
	{
		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);
	}
}
