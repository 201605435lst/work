using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.EquipmentTeam;
using SnAbp.ConstructionBase.Dtos.Procedure;
using SnAbp.ConstructionBase.Dtos.Worker;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using SnAbp.Identity;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;


namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class ProcedureAppService :
		CrudAppService<
			Procedure, ProcedureDto, Guid, ProcedureSearchDto, ProcedureCreateDto,
			ProcedureUpdateDto>,
		IProcedureAppService
	{
		private readonly IRepository<ConstructionBaseMaterial, Guid> _matRepo;
		private readonly IRepository<EquipmentTeam, Guid> _equipRepo;
		private readonly IRepository<Worker, Guid> _workerRepo;
		private readonly IRepository<ProcedureMaterial, Guid> _procedureMaterialRepo;
		private readonly IRepository<ProcedureWorker, Guid> _procedureWorkerRepo;
		private readonly IRepository<ProcedureEquipmentTeam, Guid> _procedureEquipmentTeamRepo;
		private readonly IRepository<ProcedureRltFile, Guid> _procedureRltFileRepo;
		private readonly IGuidGenerator _guidGenerator;
		private readonly IRepository<DataDictionary, Guid> _dataDictionaryRepository;

		public ProcedureAppService(
			IRepository<Procedure, Guid> repository,
			IRepository<ConstructionBaseMaterial, Guid> matRepo,
			IRepository<EquipmentTeam, Guid> equipRepo,
			IRepository<Worker, Guid> workerRepo,
			IRepository<ProcedureMaterial, Guid> procedureMaterialRepo,
			IRepository<ProcedureWorker, Guid> procedureWorkerRepo,
			IRepository<ProcedureEquipmentTeam, Guid> procedureEquipmentTeamRepo,
			IRepository<ProcedureRltFile, Guid> procedureRltFileRepo,
			IGuidGenerator guidGenerator,
			IRepository<DataDictionary, Guid> dataDictionaryRepository
		) : base(repository)
		{
			_matRepo = matRepo;
			_equipRepo = equipRepo;
			_workerRepo = workerRepo;
			_procedureMaterialRepo = procedureMaterialRepo;
			_procedureWorkerRepo = procedureWorkerRepo;
			_procedureEquipmentTeamRepo = procedureEquipmentTeamRepo;
			_procedureRltFileRepo = procedureRltFileRepo;
			_guidGenerator = guidGenerator;
			_dataDictionaryRepository = dataDictionaryRepository;
		}

		protected override void MapToEntity(ProcedureUpdateDto updateInput, Procedure entity)
		{
			if (!_dataDictionaryRepository.WithDetails()
				.Where(x => x.Key.Contains("Progress.ProjectType."))
				.Select(x => x.Id).Contains(updateInput.TypeId))
			{
				throw new UserFriendlyException("请输入正确的工序类型id!");
			}

			if (updateInput.TypeId == Guid.Empty)
			{
				throw new UserFriendlyException("请选择工序类型!");
			}

			if (updateInput.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("请输入工序名称!");
			}

			base.MapToEntity(updateInput, entity);
		}

		/// <summary>
		/// 在创建(map)之前  检查下 数据源
		/// </summary>
		/// <param name="createInput"></param>
		/// <returns></returns>
		protected override Procedure MapToEntity(ProcedureCreateDto createInput)
		{
			List<Guid> queryable = _dataDictionaryRepository.WithDetails()
				.Where(x => x.Key.Contains("Progress.ProjectType."))
				.Select(x => x.Id).ToList();
			if (!queryable.Contains(createInput.TypeId))
			{
				throw new UserFriendlyException("请输入正确的工序类型id!");
			}

			if (createInput.TypeId == Guid.Empty)
			{
				throw new UserFriendlyException("请选择工序类型!");
			}

			if (createInput.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("请输入工序名称!");
			}

			return base.MapToEntity(createInput);
		}

		protected override IQueryable<Procedure> CreateFilteredQuery(ProcedureSearchDto input)
		{
			return Repository.WithDetails()
				.WhereIf(!(input.TypeId == Guid.Empty), x => x.TypeId == input.TypeId)
				.WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name));
		}


		public async Task<ProcedureDto> ConfigProcedure(Guid id, ProcedureConfigDto input)
		{
			if (!Repository.Any(x => x.Id == id))
			{
				throw new UserFriendlyException("工序不存在!");
			}

			// 获取 这些管理表
			List<ProcedureMaterial> procedureMaterials =
				_procedureMaterialRepo.Where(x => x.ProcedureId == id).ToList();
			List<ProcedureEquipmentTeam> procedureEquipmentTeams =
				_procedureEquipmentTeamRepo.Where(x => x.ProcedureId == id).ToList();
			List<ProcedureWorker> procedureWorkers = _procedureWorkerRepo.Where(x => x.ProcedureId == id).ToList();
			List<ProcedureRltFile> procedureRtlFiles = _procedureRltFileRepo.Where(x => x.ProcedureId == id).ToList();
			//  批量删除 
			procedureMaterials.ForEach(x => { _procedureMaterialRepo.DeleteAsync(x); });
			procedureEquipmentTeams.ForEach(x => { _procedureEquipmentTeamRepo.DeleteAsync(x); });
			procedureWorkers.ForEach(x => { _procedureWorkerRepo.DeleteAsync(x); });
			procedureRtlFiles.ForEach(x => { _procedureRltFileRepo.DeleteAsync(x); });
			//  批删除量添加 
			input.MaterialIds.ForEach(x =>
				_procedureMaterialRepo.InsertAsync(new ProcedureMaterial(_guidGenerator.Create())
					{MaterialId = x.MaterialId, Count = x.Count, ProcedureId = id}));
			input.EquipmentTeamIds.ForEach(x =>
				_procedureEquipmentTeamRepo.InsertAsync(new ProcedureEquipmentTeam(_guidGenerator.Create())
					{EquipmentTeamId = x, ProcedureId = id}));
			input.WorkerIds.ForEach(x => _procedureWorkerRepo.InsertAsync(new ProcedureWorker(_guidGenerator.Create())
				{WorkerId = x, ProcedureId = id}));
			input.FileIds.ForEach(x => _procedureRltFileRepo.InsertAsync(new ProcedureRltFile(_guidGenerator.Create())
				{FileId = x, ProcedureId = id}));
			Procedure updateAsync = await Repository.GetAsync(id);
			return ObjectMapper.Map<Procedure, ProcedureDto>(updateAsync);
		}

		public async Task<ProcedureRtlObj> GetRltList(Guid id)
		{
			List<Worker> workers = await _workerRepo.GetListAsync();
			List<EquipmentTeam> equipmentTeams = await _equipRepo.GetListAsync();
			List<ConstructionBaseMaterial> materials = await _matRepo.GetListAsync();

			// 根据 工序 id  获取  工序 - 工程量 关联 表  
			List<ProcedureMaterial> procedureMaterials =
				_procedureMaterialRepo.Where(x => x.ProcedureId == id).ToList();
			List<ProcedureMaterialDto> procedureMaterialDtos =
				ObjectMapper.Map<List<ConstructionBaseMaterial>, List<ProcedureMaterialDto>>(materials);
			// 默认 的 procedureMaterial 数量是0 ，这个要根据 施工工序 id 不同 修改 对应 的 数量
			procedureMaterialDtos.ForEach(x =>
			{
				procedureMaterials.ForEach(p =>
				{
					if (p.MaterialId == x.Id)
					{
						x.Count = p.Count;
					}
				});
			});

			return new ProcedureRtlObj()
				{
					Workers = ObjectMapper.Map<List<Worker>, List<ConstructionBaseWorkerDto>>(workers),
					EquipmentTeams = ObjectMapper.Map<List<EquipmentTeam>, List<EquipmentTeamDto>>(equipmentTeams),
					Materials = procedureMaterialDtos
				}
				;
		}
	}
}