using System;
using SnAbp.ConstructionBase.Dtos.SubItem;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface ISubItemAppService
		: ICrudAppService<
			SubItemDto,
			Guid,
			SubItemSearchDto,
			SubItemCreateDto,
			SubItemUpdateDto>
	{
	}
}