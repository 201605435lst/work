
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.Standard;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	/// <summary>
	/// 工序规范维护 IService 接口
	/// </summary>
	public interface IStandardAppService : ICrudAppService< 
		StandardDto,
		Guid,
		StandardSearchDto,
		StandardCreateDto,
		StandardUpdateDto
	>
	{
		Task<List<StandardDto>> GetByIds(Guid[] ids);
	}
}
