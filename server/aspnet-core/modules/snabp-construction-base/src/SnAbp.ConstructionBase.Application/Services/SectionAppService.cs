
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.ConstructionBase.Dtos.Section;
using SnAbp.ConstructionBase.Entities;
using SnAbp.ConstructionBase.IServices;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.ConstructionBase.Services
{

	/// <summary>
	/// 未写注释 Service 
	/// </summary>
	// [Authorize]
	public class SectionAppService : CrudAppService<
			Section, SectionDto, Guid, SectionSearchDto, SectionCreateDto,
			SectionUpdateDto>,
		ISectionAppService
	{

		public SectionAppService(IRepository<Section, Guid> repository) : base(repository)
		{
			repository.GetListAsync();
		}

		/// <summary>
		/// 更新前的数据验证
		/// </summary>
		protected override void MapToEntity(SectionUpdateDto updateInput, Section entity)
		{
			Console.WriteLine("更新前验证数据");
			base.MapToEntity(updateInput, entity);
		}
		/// <summary>
		/// 创建前的数据验证
		/// </summary>
		protected override Section MapToEntity(SectionCreateDto updateInput)
		{
			Console.WriteLine("创建前验证数据");
			return base.MapToEntity(updateInput);
		}


		/// <summary>
		/// 在 getList 方法 前 构造 query.where 重写
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override IQueryable<Section> CreateFilteredQuery(SectionSearchDto input)
		{
			IQueryable<Section> query = Repository.WithDetails();

	
			// 拼接where查询
			query = query
				.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(),
				x => x.Name.Contains(input.SearchKey) ||x.Desc.Contains(input.SearchKey)  );
	
 

			return query;
		}
		
		/// <summary>
		/// 获取 树列表 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
        public async Task<PagedResultDto<SectionDto> > GetTreeListAsync(SectionSearchDto input)
        {
            var name = "";
            if (input != null && !string.IsNullOrEmpty(input.SearchKey))
            {
                name = input.SearchKey;
            }

            var ddList = await Repository.GetListAsync(true);
			// 获取总数
			int total = Repository.Count();

            List<Section> ddKeyList = ddList.FindAll(m => m.ParentId == null && m.Name != null && m.Name.Contains(name));
            ddKeyList = ddKeyList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<SectionDto> sectionDtos = ObjectMapper.Map<List<Section>, List<SectionDto>>(ddKeyList);
            return new PagedResultDto<SectionDto>(total, sectionDtos)  ;
        }

		public async Task<List<SectionDto>> GetByIds(Guid[] ids)
		{
			List<Section> sections = Repository.Where(x => ids.Contains(x.Id)).ToList();
			return ObjectMapper.Map<List<Section>, List<SectionDto>>(sections);
		}
	}
}
