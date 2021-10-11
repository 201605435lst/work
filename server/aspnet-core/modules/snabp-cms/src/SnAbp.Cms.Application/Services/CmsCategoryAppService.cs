
using SnAbp.Cms.Dtos;
using SnAbp.Cms.Entities;
using SnAbp.Cms.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Cms.Authorization;

namespace SnAbp.Cms.Services
{
    [Authorize]
    public class CmsCategoryAppService : CmsAppService, ICmsCategoryAppService
    {
        private readonly IRepository<Category, Guid> _repository;
        private readonly IDataFilter _dataFilter;

        public CmsCategoryAppService(
            IDataFilter dataFilter,
            IRepository<Category, Guid> repository)
        {
            _repository = repository;
            _dataFilter = dataFilter;
        }

        /// <summary>
        /// 获取栏目实体
        /// </summary>
        [Authorize(CmsPermissions.Category.Detail)]
        public Task<CategoryDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var category = _repository.WithDetails(x => x.Thumb).Where(x => x.Id == id).FirstOrDefault();
            if (category == null)
            {
                throw new UserFriendlyException("该栏目实体不存在");
            }

            return Task.FromResult(ObjectMapper.Map<Category, CategoryDto>(category));
        }

        /// <summary>
        /// 通过code获取栏目实体
        /// </summary>
        [Authorize(CmsPermissions.Category.Detail)]
        public Task<CategoryDto> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new UserFriendlyException("code不能为空");
            }

            var category = _repository.WithDetails(x => x.Thumb).FirstOrDefault(x => x.Code == code);
            if (category == null)
            {
                throw new UserFriendlyException("该栏目实体不存在");
            }

            return Task.FromResult(ObjectMapper.Map<Category, CategoryDto>(category));
        }

        /// <summary>
        /// 获取栏目列表
        /// </summary>
        public Task<PagedResultDto<CategoryDto>> GetList(CategorySearchDto input)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var categories = _repository.WithDetails(x => x.Thumb)
                .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
                .WhereIf(input.Enable.HasValue, x => x.Enable == input.Enable)
                .ToList();

                var treeList = GuidKeyTreeHelper<Category>.GetTree(categories);
                var result = new PagedResultDto<CategoryDto>();

                result.TotalCount = treeList.Count();
                result.Items = input.IsAll
                    ? ObjectMapper.Map<List<Category>, List<CategoryDto>>(treeList.ToList())
                    : ObjectMapper.Map<List<Category>, List<CategoryDto>>(treeList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

                return Task.FromResult(result);
            }
        }

        /// <summary>
        /// 添加栏目
        /// </summary>
        [Authorize(CmsPermissions.Category.Create)]
        public async Task<CategorySimpleDto> Create(CategoryCreateDto input)
        {
            CheckSameTitle(input.Title, null, input.ParentId);

            CheckSameCode(input.Code, null);

            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("标题不能为空");

            if (string.IsNullOrEmpty(input.Code.Trim())) throw new Volo.Abp.UserFriendlyException("标识不能为空");

            if (string.IsNullOrEmpty(input.Summary.Trim())) throw new Volo.Abp.UserFriendlyException("请输入概要");

            if (input.ThumbId == null || input.ThumbId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("缩略图不能为空");

            var categoryId = Guid.NewGuid();
            var category = new Category(categoryId);
            category.Title = input.Title;
            category.ThumbId = input.ThumbId;
            category.ParentId = input.ParentId;
            category.Code = input.Code;
            category.Summary = input.Summary;
            category.Order = input.Order;
            category.Remark = input.Remark;

            await _repository.InsertAsync(category);

            return ObjectMapper.Map<Category, CategorySimpleDto>(category);
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        [Authorize(CmsPermissions.Category.Update)]
        public async Task<CategorySimpleDto> Update(CategoryUpdateDto input)
        {
            if (input.ParentId.HasValue && input.ParentId.Value == input.Id)
            {
                throw new UserFriendlyException("栏目父级不能修改成栏目自身！！！");
            }
            CheckSameTitle(input.Title, input.Id, input.ParentId);
            CheckSameCode(input.Code, input.Id);
            var category = await _repository.GetAsync(input.Id);
            if (category == null) throw new UserFriendlyException("该栏目不存在");

            if (string.IsNullOrEmpty(input.Title.Trim())) throw new Volo.Abp.UserFriendlyException("标题不能为空");

            if (string.IsNullOrEmpty(input.Code.Trim())) throw new Volo.Abp.UserFriendlyException("标识不能为空");

            if (string.IsNullOrEmpty(input.Summary.Trim())) throw new Volo.Abp.UserFriendlyException("概要标识不能为空");

            if (input.ThumbId == null || input.ThumbId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("缩略图不能为空");

            category.Title = input.Title;
            category.ThumbId = input.ThumbId;
            category.ParentId = input.ParentId;
            category.Code = input.Code;
            category.Summary = input.Summary;
            category.Order = input.Order;
            category.Remark = input.Remark;

            await _repository.UpdateAsync(category);

            return ObjectMapper.Map<Category, CategorySimpleDto>(category);
        }

        /// <summary>
        /// 修改栏目状态
        /// </summary>
        [Authorize(CmsPermissions.Category.UpdateEnable)]
        public async Task<CategorySimpleDto> UpdateEnable(CategoryUpdateEnableDto input)
        {
            var category = await _repository.GetAsync(input.Id);
            if (category == null)
            {
                throw new UserFriendlyException("该栏目不存在");
            }
            category.Enable = input.Enable;

            await _repository.UpdateAsync(category);

            return ObjectMapper.Map<Category, CategorySimpleDto>(category);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        [Authorize(CmsPermissions.Category.Delete)]
        public Task<bool> Delete(Guid id)
        {
            return deleteCategory(id);
        }

        /// <summary>
        /// 私有方法
        /// </summary>
        // 判断是否存在相同标识的栏目
        private bool CheckSameCode(string code, Guid? id)
        {
            var sameCategories = _repository.Where(o => o.Code.ToUpper() == code.ToUpper());

            if (id.HasValue)
            {
                sameCategories = sameCategories.Where(o => o.Id != id.Value);
            }

            if (sameCategories.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("已存在相同标识的栏目！！！");
            }

            return true;
        }

        //判断是否存在相同标题的栏目
        private bool CheckSameTitle(string title, Guid? id, Guid? parentId)
        {
            var sameCategories = _repository.Where(o => o.Title.ToUpper() == title.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                sameCategories = sameCategories.Where(x => x.ParentId == parentId);
            }
            else
            {
                sameCategories = sameCategories.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameCategories = sameCategories.Where(o => o.Id != id.Value);
            }

            if (sameCategories.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前栏目分类中已存在相同标题的栏目！！！");
            }

            return true;
        }
        [Authorize(CmsPermissions.Category.Delete)]
        //删除栏目
        private async Task<bool> deleteCategory(Guid id)
        {
            var category = _repository.WithDetails(x => x.Children).Where(x => x.Id == id).FirstOrDefault();

            if (category == null)
            {
                throw new UserFriendlyException("未找到该栏目，或已被删除！！！");
            }

            if (category.Children != null && category.Children.Count() > 0)
            {
                foreach (var item in category.Children)
                {
                    await deleteCategory(item.Id);
                }
            }
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
