
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Exam.Authorization;

namespace SnAbp.Exam.Services
{
    [Authorize]
    public class ExamCategoryAppService : ExamAppService,IExamCategoryAppService
    {
        private readonly IRepository<Category, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;

        public ExamCategoryAppService(IRepository<Category,Guid> repository,IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }

        [Authorize(ExamPermissions.Category.Create)]
        public async Task<ExamCategoryDto> Create(ExamCategoryCreateDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new UserFriendlyException("分类名称不能为空");
            var category = new Category(_guidGenerator.Create());
            SaveCheckSameName(dto.Name, null, dto.ParentId);
            int order = (int)dto.Order;
            if (dto.Order < 0 || order != dto.Order) throw new UserFriendlyException("分类的排序输入错误");
            category.Name = dto.Name;
            category.ParentId = dto.ParentId;
            category.Description = dto.Description;
            category.Order = dto.Order;
            var res = await _repository.InsertAsync(category);
            return ObjectMapper.Map<Category, ExamCategoryDto>(category);
        }

        [Authorize(ExamPermissions.Category.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var category = await _repository.GetAsync(id);
            if (string.IsNullOrEmpty(category.Name)) throw new UserFriendlyException("该分类不存在");
            var _category = _repository.Where(x => id == category.Id).ToList();
            var data = GetChildren(_category,id);
            if (data.Count() > 0) throw new UserFriendlyException("需要先删除下级分类");
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<ExamCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var category = await _repository.GetAsync(id);
            if (category == null) throw new UserFriendlyException("该分类不存在");
            return ObjectMapper.Map<Category, ExamCategoryDto>(category);
        }

        public Task<PagedResultDto<ExamCategoryDto>> GetList(ExamCategorySearchDto dto)
        {

            var categories = _repository
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(dto.Name),x => string.IsNullOrEmpty(dto.Name)|| x.Name.Contains(dto.Name));
            var dataList = new List<Category>();
            if (dto.Name == null)
            {
                dataList = GetChildren(categories, null);
            }
            else
            {
                dataList = GetChildrenByName(categories, dto.Name);
            }
            var result = new PagedResultDto<ExamCategoryDto>();
            result.TotalCount = dataList.Count();
            result.Items = ObjectMapper.Map<List<Category>, List<ExamCategoryDto>>(dataList.Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList());
            return  Task.FromResult(result);
        }

        [Authorize(ExamPermissions.Category.Update)]
        public async Task<ExamCategoryDto> Update(ExamCategoryUpdateDto dto)
        {
            if (dto.Id == null || dto.Id == Guid.Empty) throw new UserFriendlyException("请输入分类的id");
            var category = await _repository.GetAsync(dto.Id);
            if (category == null) throw new UserFriendlyException("该分类不存在");
            if (string.IsNullOrEmpty(dto.Name.Trim())) throw new UserFriendlyException("请输入分类名称");
            SaveCheckSameName(dto.Name, dto.Id, dto.ParentId);
            category.Name = dto.Name;
            category.Description = dto.Description;
            category.Order = dto.Order;
            category.ParentId = dto.ParentId;
            var result = await _repository.UpdateAsync(category);
            return ObjectMapper.Map<Category, ExamCategoryDto>(category);
        }

        private bool SaveCheckSameName(string name,Guid?id,Guid?parentId)
        {
            var sameCategories = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                sameCategories = sameCategories.Where(x=>x.ParentId == parentId);
            }
            else
            {
                sameCategories = sameCategories.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameCategories = sameCategories.Where(o=>o.Id != id.Value);
            }
            if (sameCategories.Count()>0)
            {
                throw new Volo.Abp.UserFriendlyException("当前分类中存在相同名称的分类");
            }
            return true;
        }

        private List<Category> GetChildren(IEnumerable<Category>data,Guid?Id)
        {
            List<Category> list = new List<Category>();
            var children = data.Where(p => p.ParentId == Id);
            foreach (var item in children)
            {
                var node = new Category(item.Id);
                node.Name = item.Name;
                node.Description = item.Description;
                node.CreationTime = item.CreationTime;
                node.Order = item.Order;
                node.ParentId = item.ParentId;
                node.Children = GetChildren(data, node.Id);
                list.Add(node);
            }
            return list;

        }
        private List<Category> GetChildrenByName(IEnumerable<Category> data,string name)
        {
            List<Category> list = new List<Category>();
            var children = data.Where(p => p.Name.Contains(name));
            foreach (var item in children)
            {
                var node = new Category(item.Id);
                node.Name = item.Name;
                node.Description = item.Description;
                node.CreationTime = item.CreationTime;
                node.Order = item.Order;
                node.ParentId = item.ParentId;
                list.Add(node);
            }
            return list;

        }

        public async Task<List<ExamCategoryDto>> GetTreeList(Guid? parentId)
        {
            //throw new NotImplementedException();
            var list = await _repository.GetListAsync();
            list = GuidKeyTreeHelper<Category>.GetTree(list);
            return ObjectMapper.Map<List<Category>, List<ExamCategoryDto>>(list);

        }


        // 按需加载接口
        /// <summary>
        /// 根据父级Id获取试卷分类
        /// </summary>
        public Task<List<ExamCategoryDto>> GetByParentId(Guid? parentId)
        {
            if (parentId == null)
            {
                return Task.FromResult(ObjectMapper.Map<List<Category>, List<ExamCategoryDto>>(_repository.Where(a => a.Parent == null).ToList()));
            }
            else
            {
                return Task.FromResult(ObjectMapper.Map<List<Category>, List<ExamCategoryDto>>(_repository.Where(a => a.ParentId == parentId).ToList()));
            }
        }


        /// <summary>
        /// 根据分类Id获取其兄弟及父级以上的元素...
        /// </summary>
        
        public Task<List<ExamCategoryDto>> GetParentsByIds(List<Guid> ids)
        {
            var categories = new List<Category>();
            var category = new List<Category>();

            
            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    categories.AddRange(GetParents(id,category));
                }
            }

            var simpleCategory = categories.Distinct().ToList();//去除重复分类

            simpleCategory.AddRange(_repository.Where(x => x.ParentId == null).ToList());

            var treeList = GuidKeyTreeHelper<Category>.GetTree(simpleCategory);

            return Task.FromResult(ObjectMapper.Map<List<Category>, List<ExamCategoryDto>>(treeList));
        }


        //获取子集产品分类
        private List<Category> GetParents(Guid? id, List<Category> categories)
        {
            categories.AddRange(_repository.Where(x => x.ParentId == id));

            var category = _repository.FirstOrDefault(x => x.Id == id);

            if (category.ParentId != null)
            {
                GetParents(category.ParentId, categories);
            }

            return categories;
        }



    }
}
