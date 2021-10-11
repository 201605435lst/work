using System;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.ConstructionBase
{
	/// <summary>
	/// 这是单元测试的 种子 ,不要弄错！
	/// </summary>
	public class ConstructionBaseDataSeedContributor : IDataSeedContributor, ITransientDependency
	{
		private readonly IRepository<Worker, Guid> _workerRepo;
		private readonly IGuidGenerator _guidGenerator;

		public ConstructionBaseDataSeedContributor(
			IRepository<Worker, Guid> workerRepo,
			IGuidGenerator guidGenerator)
		{
			_workerRepo = workerRepo;
			_guidGenerator = guidGenerator;
		}

		public async Task SeedAsync(DataSeedContext context)
		{
			// 插入种子数据 
			if (await _workerRepo.GetCountAsync() <= 0)
			{
				await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "电工"});
				await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "混凝土工"});
				await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "钳工"});
				await _workerRepo.InsertAsync(new Worker(_guidGenerator.Create()) {Name = "木工"});
			}
		}
	}
}