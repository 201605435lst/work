
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;
using SnAbp.Construction.IServices;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Construction.Services
{

    /// <summary>
    /// 派工单模板 Service 
    /// </summary>
    // [Authorize]
    public class DispatchTemplateAppService : CrudAppService<
            DispatchTemplate, DispatchTemplateDto, Guid, DispatchTemplateSearchDto, DispatchTemplateCreateDto,
            DispatchTemplateUpdateDto>,
        IDispatchTemplateAppService
    {
        private readonly IRepository<DispatchTemplate, Guid> _dispatchTemplate;
        private readonly IGuidGenerator _guidGenerator;

        public DispatchTemplateAppService(
            IRepository<DispatchTemplate, Guid> repository,
            IRepository<DispatchTemplate, Guid> dispatchTemplate,
            IGuidGenerator guidGenerator
        ) : base(repository)
        {
            _dispatchTemplate = dispatchTemplate;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 更新前的数据验证
        /// </summary>
        protected override void MapToEntity(DispatchTemplateUpdateDto updateInput, DispatchTemplate entity)
        {
            Console.WriteLine("更新前验证数据");
            base.MapToEntity(updateInput, entity);
        }
        /// <summary>
        /// 创建前的数据验证
        /// </summary>
        protected override DispatchTemplate MapToEntity(DispatchTemplateCreateDto updateInput)
        {
            Console.WriteLine("创建前验证数据");
            return base.MapToEntity(updateInput);
        }


        /// <summary>
        /// 列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<DispatchTemplateDto>> GetAllList(DispatchTemplateSearchDto input)
        {
            var dispatchTemplates = _dispatchTemplate.WithDetails()
                .WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Description.Contains(input.SearchKey));

            var result = new PagedResultDto<DispatchTemplateDto>();
            dispatchTemplates = dispatchTemplates.OrderBy(x => x.CreationTime);
            result.TotalCount = dispatchTemplates.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<DispatchTemplate>, List<DispatchTemplateDto>>(dispatchTemplates.ToList());
            }
            else
            {
                result.Items = ObjectMapper.Map<List<DispatchTemplate>, List<DispatchTemplateDto>>(dispatchTemplates.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return Task.FromResult(result);

        }

        /// <summary>
        /// 列表默认模板名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<string> GetDefaultName()
        {
            var dispatchTemplate = _dispatchTemplate.FirstOrDefault(x => x.IsDefault);

            return Task.FromResult(dispatchTemplate?.Name);
        }


        /// <summary>
        /// 在 getList 方法 前 构造 query.where 重写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //protected override IQueryable<DispatchTemplate> CreateFilteredQuery(DispatchTemplateSearchDto input)
        //{
        //    IQueryable<DispatchTemplate> query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

        //    query = query
        //        .WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.Description.Contains(input.SearchKey)); // 模糊查询
        //                                                                                                                                           // 	.WhereIf(input.ProjectId.HasValue, x => x.ProjectId == input.ProjectId); // 根据xx类型查询 
        //    ;
        //    return query.OrderBy(x=>x.CreationTime);
        //}
        /// <summary>
        /// 创建
        /// </summary>
        public override async Task<DispatchTemplateDto> CreateAsync(DispatchTemplateCreateDto input)
        {
            if (input.Name == null) throw new UserFriendlyException("模板名称不能为空");
            if (input.IsDefault)
            {
                var query = _dispatchTemplate.FirstOrDefault(x => x.IsDefault);
                if (query != null) { query.IsDefault = false; }
            }
            var dispatch = new DispatchTemplate(_guidGenerator.Create())
            {
                Name = input.Name,
                Description = input.Description,
                IsDefault = input.IsDefault,
                Remark = input.Remark,
            };
            await _dispatchTemplate.InsertAsync(dispatch);
            return new DispatchTemplateDto();
        }
        public override async Task<DispatchTemplateDto> UpdateAsync(Guid id, DispatchTemplateUpdateDto input)
        {

            if (input.Name == null) throw new UserFriendlyException("模板名称不能为空");
            if (input.IsDefault)
            {
                var query = _dispatchTemplate.FirstOrDefault(x => x.IsDefault);
                if (query != null) { query.IsDefault = false; }
            }
            var dispatch = _dispatchTemplate.FirstOrDefault(x => x.Id == id);
            dispatch.Name = input.Name;
            dispatch.Description = input.Description;
            dispatch.IsDefault = input.IsDefault;
            dispatch.Remark = input.Remark;
            await _dispatchTemplate.UpdateAsync(dispatch);
            return new DispatchTemplateDto();
        }

        /// <summary>
        /// 设置默认
        /// </summary>
        public async Task<bool> setDefault(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }
            var query = _dispatchTemplate.FirstOrDefault(x => x.IsDefault);
            if (query != null) { query.IsDefault = false; }
            var queryId = _dispatchTemplate.FirstOrDefault(x => x.Id == id);
            if (queryId != null) { queryId.IsDefault = true; }

            await _dispatchTemplate.UpdateAsync(queryId);
            return true;
        }
        /// <summary>
        /// 删除多个 派工单模板
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            await Repository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }


    }
}
