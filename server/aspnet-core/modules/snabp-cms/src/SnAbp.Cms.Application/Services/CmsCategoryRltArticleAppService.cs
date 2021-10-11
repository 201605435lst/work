using SnAbp.Cms.Dto.CategoryRltArticle;
using SnAbp.Cms.Entities;
using SnAbp.Cms.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Cms.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Cms.Authorization;

namespace SnAbp.Cms.Services
{
    [Authorize]
    public class CmsCategoryRltArticleAppService : CmsAppService, ICmsCategoryRltArticleAppService
    {
        private readonly IRepository<CategoryRltArticle, Guid> _repository;
        public CmsCategoryRltArticleAppService(IRepository<CategoryRltArticle, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取栏目文章实体
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.Detail)]
        public Task<CategoryRltArticleSimpleDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var categoryRltArticle = _repository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (categoryRltArticle == null)
            {
                throw new UserFriendlyException("该栏目文章不存在");
            }

            return Task.FromResult(ObjectMapper.Map<CategoryRltArticle, CategoryRltArticleSimpleDto>(categoryRltArticle));
        }

        /// <summary>
        /// 获取栏目文章列表
        /// </summary>
        public Task<PagedResultDto<CategoryRltArticleSimpleDto>> GetList(CategoryRltArticleSearchDto input)
        {
            var categoryRltArticles = _repository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Article.Title.Contains(input.Title))
                .WhereIf(input.Enable.HasValue, x => x.Enable == input.Enable)
                .WhereIf(input.CategoryIds != null && input.CategoryIds.Count() > 0, x => input.CategoryIds.Contains(x.CategoryId));

            var result = new PagedResultDto<CategoryRltArticleSimpleDto>();
            result.TotalCount = categoryRltArticles.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<CategoryRltArticle>, List<CategoryRltArticleSimpleDto>>(categoryRltArticles.OrderByDescending(s => s.Category.Title).ThenByDescending(x => x.Order).ToList());
            }
            else
            {
                result.Items = ObjectMapper.Map<List<CategoryRltArticle>, List<CategoryRltArticleSimpleDto>>(categoryRltArticles.OrderByDescending(s => s.Category.Title).ThenByDescending(x => x.Order).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return Task.FromResult(result);
        }

        /// <summary>
        /// 添加栏目文章排序
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.Create)]
        public async Task<CategoryRltArticleSimpleDto> Create(CategoryRltArticleCreateDto input)
        {
            if (input.CategoryId == null || input.CategoryId == Guid.Empty) throw new UserFriendlyException("请选择栏目");

            if (input.ArticleId == null || input.ArticleId == Guid.Empty) throw new UserFriendlyException("请选择文章");

            if (_repository.Any(x => x.CategoryId == input.CategoryId && x.ArticleId == input.ArticleId)) throw new UserFriendlyException("该栏目文章已存在");


            var categoryRltArticleId = Guid.NewGuid();
            var categoryRltArticle = new CategoryRltArticle(categoryRltArticleId);
            categoryRltArticle.CategoryId = input.CategoryId;
            categoryRltArticle.ArticleId = input.ArticleId;
            categoryRltArticle.Order = input.Order;

            await _repository.InsertAsync(categoryRltArticle);


            return ObjectMapper.Map<CategoryRltArticle, CategoryRltArticleSimpleDto>(categoryRltArticle);
        }

        /// <summary>
        /// 修改栏目文章
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.Update)]
        public async Task<CategoryRltArticleSimpleDto> Update(CategoryRltArticleUpdateDto input)
        {
            var categoryRltArticle = await _repository.GetAsync(input.Id);
            if (categoryRltArticle == null)
            {
                throw new UserFriendlyException("该栏目文章不存在");
            }

            categoryRltArticle.CategoryId = input.CategoryId;
            categoryRltArticle.ArticleId = input.ArticleId;
            categoryRltArticle.Order = input.Order;

            await _repository.UpdateAsync(categoryRltArticle);

            return ObjectMapper.Map<CategoryRltArticle, CategoryRltArticleSimpleDto>(categoryRltArticle);
        }

        /// <summary>
        /// 修改栏目文章启用状态
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.UpdateEnable)]
        public async Task<CategoryRltArticleSimpleDto> UpdateEnable(CategoryRltArticleUpdateEnableDto input)
        {
            var categoryRltArticle = await _repository.GetAsync(input.Id);
            if (categoryRltArticle == null)
            {
                throw new UserFriendlyException("该栏目文章不存在");
            }
            categoryRltArticle.Enable = input.Enable;

            await _repository.UpdateAsync(categoryRltArticle);

            return ObjectMapper.Map<CategoryRltArticle, CategoryRltArticleSimpleDto>(categoryRltArticle);
        }

        /// <summary>
        /// 修改栏目文章排序
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.UpdateOrder)]
        public async Task<CategoryRltArticleSimpleDto> UpdateOrder(CategoryRltArticleUpdateOrderDto input)
        {
            var categoryRltArticle = await _repository.GetAsync(input.Id);
            if (categoryRltArticle == null)
            {
                throw new UserFriendlyException("该栏目文章不存在");
            }
            categoryRltArticle.Order = input.Order;

            await _repository.UpdateAsync(categoryRltArticle);

            return ObjectMapper.Map<CategoryRltArticle, CategoryRltArticleSimpleDto>(categoryRltArticle);
        }

        /// <summary>
        /// 删除栏目文章
        /// </summary>
        [Authorize(CmsPermissions.CategoryRltArticle.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            //foreach (var id in ids)
            //{
            //    var categoryRltArticle = _repository.GetAsync(id);
            //    if (categoryRltArticle == null)
            //    {
            //        throw new UserFriendlyException("未找到该栏目文章，或已被删除！！！");
            //    }
            //}
            await _repository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
    }
}
