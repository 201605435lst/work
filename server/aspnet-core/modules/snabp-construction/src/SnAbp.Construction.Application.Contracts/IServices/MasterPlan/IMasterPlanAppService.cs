using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlan;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices.MasterPlan
{
	/// <summary>
	/// 施工计划 IService 接口
	/// </summary>
	public interface IMasterPlanAppService : ICrudAppService< 
		MasterPlanDto,
		Guid,
		MasterPlanSearchDto,
		MasterPlanCreateDto,
		MasterPlanUpdateDto
	>
	{
		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);
	}
}
