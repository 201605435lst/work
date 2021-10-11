
using System;
using SnAbp.Construction.Dtos.PlanMaterial;
using Volo.Abp.Application.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices
{
	/// <summary>
	/// 施工计划工程量 IService 接口
	/// </summary>
	public interface IPlanMaterialAppService : ICrudAppService< 
		PlanMaterialDto,
		Guid,
		PlanMaterialSearchDto,
		PlanMaterialCreateDto,
		PlanMaterialUpdateDto
	>
	{
		/// <summary>
		/// 删除多个
		/// </summary>
		/// <returns></returns>
		Task<bool> DeleteRange(List<Guid> ids);
	}
}
