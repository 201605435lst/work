using Microsoft.AspNetCore.Authorization;
using SnAbp.Cms.Authorization;
using SnAbp.Cms.Dtos;
using SnAbp.Cms.Entities;
using SnAbp.Cms.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Cms.Services
{
    [Authorize]
    public class CmsArticleAppService : CmsAppService, ICmsArticleAppService
    {
        private readonly IRepository<Article, Guid> _repository;
        private readonly IRepository<ArticleAccessory, Guid> _repositoryArticleAccessory;
        private readonly IRepository<ArticleCarousel, Guid> _repositoryArticleCarousel;
        private readonly IRepository<CategoryRltArticle, Guid> _repositoryCategoryRltArticle;
        private readonly IDataFilter _dataFilter;
        public CmsArticleAppService(
            IRepository<Article, Guid> repository,
            IRepository<ArticleAccessory, Guid> repositoryArticleAccessory,
            IRepository<ArticleCarousel, Guid> repositoryArticleCarousel,
            IDataFilter dataFilter,
            IRepository<CategoryRltArticle, Guid> repositoryCategoryRltArticle)
        {
            _repository = repository;
            _repositoryArticleAccessory = repositoryArticleAccessory;
            _repositoryArticleCarousel = repositoryArticleCarousel;
            _repositoryCategoryRltArticle = repositoryCategoryRltArticle;
            _dataFilter = dataFilter;
        }

        /// <summary>
        /// 获取文章实体
        /// </summary>
        [Authorize(CmsPermissions.Article.Detail)]
        public Task<ArticleDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var article = _repository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (article == null)
            {
                throw new UserFriendlyException("该文章不存在");
            }

            return Task.FromResult(ObjectMapper.Map<Article, ArticleDto>(article));
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        public Task<PagedResultDto<ArticleSimpleDto>> GetList(ArticleSearchDto input)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var articles = _repository.WithDetails()
                    .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                    .WhereIf(!string.IsNullOrEmpty(input.Author), x => x.Author.Contains(input.Author))
                    .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Date >= input.StartTime && x.Date <= input.EndTime)
                    .WhereIf(input.CategoryIds != null && input.CategoryIds.Count() > 0, x => x.Categories.Any(y => input.CategoryIds.Contains(y.CategoryId)))
                    .WhereIf(!string.IsNullOrEmpty(input.CategoryCode), x => x.Categories.Any(y => y.Category.Code == input.CategoryCode));

                var result = new PagedResultDto<ArticleSimpleDto>();
                result.TotalCount = articles.Count();
                if (input.IsAll)
                {
                    result.Items = ObjectMapper.Map<List<Article>, List<ArticleSimpleDto>>(articles.OrderByDescending(x => x.CreationTime).ToList());
                }
                else
                {
                    result.Items = ObjectMapper.Map<List<Article>, List<ArticleSimpleDto>>(articles.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                }
                return Task.FromResult(result);
            }
        }

        /// <summary>
        /// 获取不属于当前栏目的所有文章
        /// </summary>
        public Task<PagedResultDto<ArticleSimpleDto>> GetOutofCategoryList(ArticleSearchOutofCategoryDto input)
        {
            var articles = _repository.WithDetails()
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => !x.Categories.Select(s => s.CategoryId).Contains(input.CategoryId))
                .ToList();
            var result = new PagedResultDto<ArticleSimpleDto>();
            result.TotalCount = articles.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<Article>, List<ArticleSimpleDto>>(articles.OrderByDescending(x => x.CreationTime).ToList());
            }
            else
            {
                result.Items = ObjectMapper.Map<List<Article>, List<ArticleSimpleDto>>(articles.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return Task.FromResult(result);

        }

        /// <summary>
        /// 创建文章
        /// </summary>
        //[Authorize(CmsPermissions.Article.Create)]
        public async Task<ArticleDto> Create(ArticleCreateDto input)
        {

            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("标题不能为空");

            if (string.IsNullOrEmpty(input.Summary.Trim())) throw new Volo.Abp.UserFriendlyException("概要不能为空");

            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("内容不能为空");

            if (string.IsNullOrEmpty(input.Author.Trim())) throw new Volo.Abp.UserFriendlyException("作者不能为空");

            CheckSameTitle(input.Title, null);//标题验证

            var articleId = Guid.NewGuid();
            var article = new Article(articleId);
            article.Title = input.Title;
            article.Summary = input.Summary;
            article.ThumbId = input.ThumbId;
            article.Content = input.Content;
            article.Author = input.Author;
            article.Date = input.Date;
            article.Categories = new List<CategoryRltArticle>();

            // 重新保存关联栏目信息
            foreach (var category in input.Categories)
            {
                article.Categories.Add(new CategoryRltArticle(Guid.NewGuid())
                {
                    CategoryId = category.Id,
                    Order = 0
                });
            }

            article.Carousels = new List<ArticleCarousel>();
            // 重新保存文章轮播图信息
            foreach (var file in input.Carousels)
            {
                article.Carousels.Add(new ArticleCarousel(Guid.NewGuid())
                {
                    FileId = file.FileId,
                    Order = file.Order
                });
            }

            article.Accessories = new List<ArticleAccessory>();
            // 重新保存附件信息
            foreach (var file in input.Accessories)
            {
                article.Accessories.Add(new ArticleAccessory(Guid.NewGuid())
                {
                    FileId = file.FileId
                });
            }

            await _repository.InsertAsync(article);


            return ObjectMapper.Map<Article, ArticleDto>(article);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        [Authorize(CmsPermissions.Article.Update)]
        public async Task<ArticleDto> Update(ArticleUpdateDto input)
        {

            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("标题不能为空");

            if (string.IsNullOrEmpty(input.Summary.Trim())) throw new Volo.Abp.UserFriendlyException("概要不能为空");

            if (string.IsNullOrEmpty(input.Content.Trim())) throw new Volo.Abp.UserFriendlyException("内容不能为空");

            if (string.IsNullOrEmpty(input.Author.Trim())) throw new Volo.Abp.UserFriendlyException("作者不能为空");

            CheckSameTitle(input.Title, input.Id);//标题验证

            var article = await _repository.GetAsync(input.Id);
            if (article == null)
            {
                throw new UserFriendlyException("该文章不存在");
            }

            var categoryRltArticles = _repositoryCategoryRltArticle.Where(x => x.ArticleId == input.Id);


            // 清楚之前关联栏目信息
            await _repositoryCategoryRltArticle.DeleteAsync(x => x.ArticleId == article.Id);

            // 重新保存关联栏目信息
            article.Categories = new List<CategoryRltArticle>();
            foreach (var category in input.Categories)
            {
                var order = 0;
                var enable = false;
                foreach (var rlt in categoryRltArticles)
                {
                    if (rlt.CategoryId == category.Id)
                    {
                        enable = rlt.Enable;
                        order = rlt.Order;
                        break;
                    }
                }
                article.Categories.Add(new CategoryRltArticle(Guid.NewGuid())
                {
                    Enable = enable,
                    Order = order,
                    CategoryId = category.Id
                });
            }

            // 清楚之前关联轮播图信息
            await _repositoryArticleCarousel.DeleteAsync(x => x.ArticleId == article.Id);
            var articleCarousels = _repositoryArticleCarousel.Where(x => x.ArticleId == input.Id);
            // 重新保存关联轮播图信息
            article.Carousels = new List<ArticleCarousel>();
            foreach (var carousel in input.Carousels)
            {
                var order = 0;
                foreach (var rlt in articleCarousels)
                {
                    if (rlt.CreatorId == carousel.Id)
                    {
                        order = rlt.Order;
                        break;
                    }
                }
                article.Carousels.Add(new ArticleCarousel(Guid.NewGuid())
                {
                    FileId = carousel.FileId,
                    Order = carousel.Order
                });
            }

            // 清楚之前关联附件信息
            await _repositoryArticleAccessory.DeleteAsync(x => x.ArticleId == article.Id);

            // 重新保存关联附件信息
            article.Accessories = new List<ArticleAccessory>();
            foreach (var accessory in input.Accessories)
            {
                article.Accessories.Add(new ArticleAccessory(Guid.NewGuid())
                {
                    FileId = accessory.FileId
                });
            }

            article.Title = input.Title;
            article.Summary = input.Summary;
            article.ThumbId = input.ThumbId;
            article.Content = input.Content;
            article.Author = input.Author;
            article.Date = input.Date;

            await _repository.UpdateAsync(article);

            return ObjectMapper.Map<Article, ArticleDto>(article);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        [Authorize(CmsPermissions.Article.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 私有方法
        /// </summary>
        //验证文章标题唯一性
        private bool CheckSameTitle(string title, Guid? id)
        {
            var sameArticles = _repository.Where(o => o.Title.ToUpper() == title.ToUpper());

            if (id.HasValue)
            {
                sameArticles = sameArticles.Where(o => o.Id != id.Value);
            }

            if (sameArticles.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("已存在相同标题的文章！！！");
            }

            return true;
        }
    }
}
