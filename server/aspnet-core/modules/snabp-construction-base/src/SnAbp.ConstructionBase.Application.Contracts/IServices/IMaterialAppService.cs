using System;
using SnAbp.ConstructionBase.Dtos.Material;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface IMaterialAppService : ICrudAppService<
		ConstructionBaseMaterialDto,
		Guid,
		MaterialSearchDto,
		ConstructionBaseMaterialCreateDto,
		ConstructionBaseMaterialUpdateDto
	>
	{
	}
}