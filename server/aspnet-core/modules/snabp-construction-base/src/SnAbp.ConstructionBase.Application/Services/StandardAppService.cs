
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.Standard;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Services
{

	/// <summary>
	/// 工序规范维护 Service 
	/// </summary>
	// [Authorize]
	public class StandardAppService : CrudAppService<
			Standard, StandardDto, Guid, StandardSearchDto, StandardCreateDto,
			StandardUpdateDto>,
		IStandardAppService
	{

		public StandardAppService(IRepository<Standard, Guid> repository) : base(repository)
		{
			repository.GetListAsync();
		}

		/// <summary>
		/// 更新前的数据验证
		/// </summary>
		protected override void MapToEntity(StandardUpdateDto updateInput, Standard entity)
		{
			Console.WriteLine("更新前验证数据");
			base.MapToEntity(updateInput, entity);
		}
		/// <summary>
		/// 创建前的数据验证
		/// </summary>
		protected override Standard MapToEntity(StandardCreateDto updateInput)
		{
			Console.WriteLine("创建前验证数据");
			return base.MapToEntity(updateInput);
		}


		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override IQueryable<Standard> CreateFilteredQuery(StandardSearchDto input)
		{
			IQueryable<Standard> query = Repository.WithDetails();

	
			// 拼接where查询
			query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(),
				x => x.Name.Contains(input.SearchKey) ||x.Code.Contains(input.SearchKey)  );
	

			
			query = query.WhereIf(input.ProfessionId.HasValue,x=>x.ProfessionId==input.ProfessionId); 


			return query;
		}

		public async Task<List<StandardDto>> GetByIds(Guid[] ids)
		{
			List<Standard> standards = Repository.Where(x => ids.Contains(x.Id)).ToList();
			return ObjectMapper.Map<List<Standard>, List<StandardDto>>(standards);
		}
	}
}
