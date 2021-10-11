using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Dtos.Procedure;
using SnAbp.ConstructionBase.Dtos.Worker;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface IProcedureAppService
		: ICrudAppService<
			ProcedureDto,
			Guid,
			ProcedureSearchDto,
			ProcedureCreateDto,
			ProcedureUpdateDto>
	{
		/// <summary>
		/// 配置 工序 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<ProcedureDto> ConfigProcedure(Guid id, ProcedureConfigDto input);

		/// <summary>
		/// 根据 工序 id  获取 关联表的  Rlt list
		/// </summary>
		/// <returns></returns>
		Task<ProcedureRtlObj> GetRltList(Guid id);
	}
}