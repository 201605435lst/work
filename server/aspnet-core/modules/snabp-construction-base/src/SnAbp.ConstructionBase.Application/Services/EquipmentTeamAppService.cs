using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using SnAbp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class EquipmentTeamAppService : ConstructionBaseAppService, IEquipmentTeamAppService
	{
		private readonly IRepository<EquipmentTeam, Guid> _equipmentTeamRepository;
		private readonly IRepository<DataDictionary, Guid> _dataDictionaryRepository;
		private readonly IGuidGenerator _guidGenerator;

		public EquipmentTeamAppService(
			IRepository<EquipmentTeam, Guid> equipmentTeamRepository,
			IRepository<DataDictionary, Guid> dataDictionaryRepository,
			IGuidGenerator guidGenerator)
		{
			_equipmentTeamRepository = equipmentTeamRepository;
			_dataDictionaryRepository = dataDictionaryRepository;
			_guidGenerator = guidGenerator;
		}

		public async Task<EquipmentTeamDto> Get(Guid id)
		{
			EquipmentTeam equipmentTeam = await _equipmentTeamRepository.GetAsync(id);
			return ObjectMapper.Map<EquipmentTeam, EquipmentTeamDto>(equipmentTeam);
		}

		public Task<PagedResultDto<EquipmentTeamDto>> GetList(EquipmentTeamSearchDto input)
		{
			IQueryable<EquipmentTeam> query = _equipmentTeamRepository.WithDetails()
					.WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
					.WhereIf(input.TypeId != Guid.Empty, x => x.TypeId == input.TypeId)
				;
			// 获取总数
			int total = query.Count();
			List<EquipmentTeam> list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
			return Task.FromResult(new PagedResultDto<EquipmentTeamDto>(total,
				ObjectMapper.Map<List<EquipmentTeam>, List<EquipmentTeamDto>>(list)));
		}

		public async Task<EquipmentTeamDto> Create(EquipmentTeamCreateDto input)
		{
			if (input.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("设备台班名称不能为空!");
			}

			if (input.TypeId == Guid.Empty)
			{
				throw new UserFriendlyException("请选择设备类型!");
			}

			if (!_dataDictionaryRepository.WithDetails()
				.Where(x => x.Key.Contains("Progress.EquipmentType."))
				.Select(x => x.Id).Contains(input.TypeId))
			{
				throw new UserFriendlyException("请输入正确的设备类型id!");
			}


			EquipmentTeam equipmentTeam = ObjectMapper.Map<EquipmentTeamCreateDto, EquipmentTeam>(input);
			EquipmentTeam insertWorker = await _equipmentTeamRepository.InsertAsync(equipmentTeam);
			EquipmentTeamDto equipmentTeamDto = ObjectMapper.Map<EquipmentTeam, EquipmentTeamDto>(insertWorker);
			return equipmentTeamDto;
		}

		public async Task<EquipmentTeamDto> Update(Guid id, EquipmentTeamUpdateDto input)
		{
			if (input.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("设备台班名称不能为空!");
			}

			if (input.TypeId == Guid.Empty)
			{
				throw new UserFriendlyException("请选择设备类型!");
			}

			if (!_dataDictionaryRepository.WithDetails()
				.Where(x => x.Key.Contains("Progress.EquipmentType."))
				.Select(x => x.Id).Contains(input.TypeId))
			{
				throw new UserFriendlyException("请输入正确的设备类型id!");
			}

			//用于IAuthorRepository.GetAsync从数据库获取作者实体。没有具有给定id的作者，
			//则GetAsync抛出EntityNotFoundException该错误，从而404在Web应用程序中导致HTTP状态代码。
			//始终使实体处于更新操作是一个好习惯。
			EquipmentTeam equipmentTeam = await _equipmentTeamRepository.GetAsync(id);
			ObjectMapper.Map(input, equipmentTeam);
			EquipmentTeam updateAsync = await _equipmentTeamRepository.UpdateAsync(equipmentTeam);
			return ObjectMapper.Map<EquipmentTeam, EquipmentTeamDto>(updateAsync);
		}

		public async Task Delete(Guid id)
		{
			await _equipmentTeamRepository.DeleteAsync(id);
		}
	}
}