using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.Material;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Services
{
	// [Authorize]
	public class MaterialAppService : CrudAppService<
			ConstructionBaseMaterial, ConstructionBaseMaterialDto, Guid, MaterialSearchDto, ConstructionBaseMaterialCreateDto,
			ConstructionBaseMaterialUpdateDto>,
		IMaterialAppService
	{
		public MaterialAppService(IRepository<ConstructionBaseMaterial, Guid> repository) : base(repository)
		{
			repository.GetListAsync();
			
		}

		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override IQueryable<ConstructionBaseMaterial> CreateFilteredQuery(MaterialSearchDto input)
		{
			IQueryable<ConstructionBaseMaterial> query =Repository.WithDetails();
			// 模糊查询 material 里面的  name 和 code 
			query = query.WhereIf(!input.Name.IsNullOrWhiteSpace(),
				x => x.Name.Contains(input.Name) || x.Code.Contains(input.Name));
			return query;
		}
	}
}