
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.Section;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	/// <summary>
	/// 未写注释 IService 接口
	/// </summary>
	public interface ISectionAppService : ICrudAppService< 
		SectionDto,
		Guid,
		SectionSearchDto,
		SectionCreateDto,
		SectionUpdateDto
	>
	{
		Task<List<SectionDto>> GetByIds(Guid[] ids);
	}
}
