using System;
using System.Threading.Tasks;
using SnAbp.ConstructionBase.Dtos.Worker;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.ConstructionBase.IServices
{
	public interface IWorkerAppService : IApplicationService
	{
		/// <summary>
		/// 根据ID获取worker
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ConstructionBaseWorkerDto> Get(Guid id);

		/// <summary>
		/// 根据条件查询 worker 
		/// </summary>
		Task<PagedResultDto<ConstructionBaseWorkerDto>> GetList(WorkerSearchDto input);

		/// <summary>
		/// 添加worker
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<ConstructionBaseWorkerDto> Create(ConstructionBaseWorkerCreateDto input);

		/// <summary>
		/// 更新worker
		/// </summary>
		/// <param name="id"></param>
		/// <param name="input"></param>
		/// <returns></returns>
		Task<ConstructionBaseWorkerDto> Update(Guid id, ConstructionBaseWorkerUpdateDto input);

		Task Delete(Guid id);
	}
}