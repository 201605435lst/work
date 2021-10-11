using System;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface IEquipmentTeamAppService : IApplicationService
	{
		/// <summary>
		/// 根据ID获取Equipment
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<EquipmentTeamDto> Get(Guid id);

		/// <summary>
		/// 根据条件查询 Equipment 
		/// </summary>
		Task<PagedResultDto<EquipmentTeamDto>> GetList(EquipmentTeamSearchDto input);

		/// <summary>
		/// 添加Equipment
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<EquipmentTeamDto> Create(EquipmentTeamCreateDto input);

		/// <summary>
		/// 更新Equipment
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<EquipmentTeamDto> Update(Guid id, EquipmentTeamUpdateDto input);

		/// <summary>
		/// 删除EquipmentTeam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task Delete(Guid id);
	}
}