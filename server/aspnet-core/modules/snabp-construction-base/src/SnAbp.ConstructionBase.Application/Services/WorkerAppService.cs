using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.Worker;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class WorkerAppService : ConstructionBaseAppService, IWorkerAppService
	{
		private readonly IRepository<Worker, Guid> _workerRepository;
		private readonly IGuidGenerator _guidGenerator;

		public WorkerAppService(IRepository<Worker, Guid> workerRepository, IGuidGenerator guidGenerator)
		{
			_workerRepository = workerRepository;
			_guidGenerator = guidGenerator;
		}

		public async Task<ConstructionBaseWorkerDto> Get(Guid id)
		{
			Worker worker = await _workerRepository.GetAsync(id);
			return ObjectMapper.Map<Worker, ConstructionBaseWorkerDto>(worker);
		}

		public Task<PagedResultDto<ConstructionBaseWorkerDto>> GetList(WorkerSearchDto input)
		{
			IQueryable<Worker> query = _workerRepository.WithDetails()
				.WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name));
			// 获取总数
			int total = query.Count();
			List<Worker> list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
			return Task.FromResult(new PagedResultDto<ConstructionBaseWorkerDto>(total,
				ObjectMapper.Map<List<Worker>, List<ConstructionBaseWorkerDto>>(list)));
		}

		public async Task<ConstructionBaseWorkerDto> Create(ConstructionBaseWorkerCreateDto input)
		{
			if (input.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("工种信息名称不能为空!");
			}

			Worker worker1 = new Worker(_guidGenerator.Create()) {Name = input.Name};
			Worker insertWorker = await _workerRepository.InsertAsync(worker1);
			ConstructionBaseWorkerDto workerDto = ObjectMapper.Map<Worker, ConstructionBaseWorkerDto>(insertWorker);
			return workerDto;
		}

		public async Task<ConstructionBaseWorkerDto> Update(Guid id, ConstructionBaseWorkerUpdateDto input)
		{
			//用于IAuthorRepository.GetAsync从数据库获取作者实体。如果没有具有给定id的作者，
			//则GetAsync抛出EntityNotFoundException该错误，从而404在Web应用程序中导致HTTP状态代码。
			//始终使实体处于更新操作是一个好习惯。
			Worker worker = await _workerRepository.GetAsync(id);
			if (input.Name.IsNullOrWhiteSpace())
			{
				throw new UserFriendlyException("工种信息名称不能为空!");
			}
			ObjectMapper.Map(input, worker);
			Worker updateAsync = await _workerRepository.UpdateAsync(worker);
			return ObjectMapper.Map<Worker, ConstructionBaseWorkerDto>(updateAsync);
		}

		public async Task Delete(Guid id)
		{
			await _workerRepository.DeleteAsync(id);
			
		}
	}
}