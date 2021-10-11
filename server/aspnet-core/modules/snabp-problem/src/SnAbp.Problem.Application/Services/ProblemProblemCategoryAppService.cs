using SnAbp.Problem.Dtos;
using SnAbp.Problem.Entities;
using SnAbp.Problem.IServices;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Problem.Services
{
    public class ProblemProblemCategoryAppService : ProblemAppService, IProblemProblemCategoryAppService

    {
        private readonly IRepository<ProblemCategory, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        public ProblemProblemCategoryAppService(
            IRepository<ProblemCategory, Guid> repository,
            IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProblemCategoryDto> Get(Guid id)
        {

            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var problemCategory = _repository.WithDetails(x => x.Parent).Where(x => x.Id == id).FirstOrDefault();
            if (problemCategory == null)
            {
                throw new UserFriendlyException("该问题实体不存在");
            }
            return Task.FromResult(ObjectMapper.Map<ProblemCategory, ProblemCategoryDto>(problemCategory));
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ProblemCategoryDto>> GetList(ProblemCategorySearchDto input)
        {
            var problemCategories = _repository.WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name)).ToList();

            var treeList = GuidKeyTreeHelper<ProblemCategory>.GetTree(problemCategories);

            var result = new PagedResultDto<ProblemCategoryDto>()
            {
                TotalCount = treeList.Count(),
                Items = input.IsAll
                ? ObjectMapper.Map<List<ProblemCategory>, List<ProblemCategoryDto>>(treeList.ToList())
                : ObjectMapper.Map<List<ProblemCategory>, List<ProblemCategoryDto>>(treeList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProblemCategoryDto> Create(ProblemCategoryCreateDto input)
        {
            CheckSameName(input.Name, null, input.ParentId);

            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("名称不能为空");

            var problemCategory = new ProblemCategory(_guidGenerator.Create())
            {
                Name = input.Name,
                Order = input.Order,
                ParentId = input.ParentId,
            };

            await _repository.InsertAsync(problemCategory);

            return ObjectMapper.Map<ProblemCategory, ProblemCategoryDto>(problemCategory);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProblemCategoryDto> Update(ProblemCategoryUpdateDto input)
        {
            if (input.ParentId.HasValue && input.ParentId.Value == input.Id)
            {
                throw new UserFriendlyException("栏目父级不能修改成栏目自身！！！");
            }
            CheckSameName(input.Name, input.Id, input.ParentId);

            var problemCategory = await _repository.GetAsync(input.Id);
            if (problemCategory == null) throw new UserFriendlyException("该问题分类不存在");

            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("名称不能为空");

            problemCategory.Name = input.Name;
            problemCategory.Order = input.Order;
            problemCategory.ParentId = input.ParentId;

            await _repository.UpdateAsync(problemCategory);

            return ObjectMapper.Map<ProblemCategory, ProblemCategoryDto>(problemCategory);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");

            var problemCategory = _repository.WithDetails(x => x.Children).Where(x => x.Id == id).FirstOrDefault();

            if (problemCategory == null) throw new UserFriendlyException("该问题分类不存在");

            if (problemCategory.Children != null && problemCategory.Children.Count > 0) throw new UserFriendlyException("请先删除该问题分类的下级");

            await _repository.DeleteAsync(id);

            return true;
        }


        private bool CheckSameName(string name, Guid? id, Guid? parentId)
        {
            var sameProblemCategories = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                sameProblemCategories = sameProblemCategories.Where(x => x.ParentId == parentId);
            }
            else
            {
                sameProblemCategories = sameProblemCategories.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameProblemCategories = sameProblemCategories.Where(o => o.Id != id.Value);
            }

            if (sameProblemCategories.Count() > 0)
            {
                throw new UserFriendlyException("当前问题分类中已存在相同名称，请重新输入！");
            }

            return true;
        }
    }
}
