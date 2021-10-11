using SnAbp.Problem.Dtos;
using SnAbp.Problem.Entities;
using SnAbp.Problem.IServices;
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
    public class ProblemProblemAppService : ProblemAppService, IProblemProblemAppService
    {
        private readonly IRepository<Entities.Problem, Guid> _repository;
        private readonly IRepository<ProblemRltProblemCategory, Guid> _ProblemRltProblemCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;
        public ProblemProblemAppService(
            IRepository<Entities.Problem, Guid> repository,
            IRepository<ProblemRltProblemCategory, Guid> ProblemRltProblemCategoryRepository,
            IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _ProblemRltProblemCategoryRepository = ProblemRltProblemCategoryRepository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProblemDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var problem = _repository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (problem == null)
            {
                throw new UserFriendlyException("该问题实体不存在");
            }
            return Task.FromResult(ObjectMapper.Map<Entities.Problem, ProblemDto>(problem));
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ProblemDto>> GetList(ProblemSearchDto input)
        {
            var problems = _repository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Content.Contains(input.Keywords))
                .WhereIf(input.ProblemCategoryIds != null && input.ProblemCategoryIds.Count() > 0,
                         x => x.ProblemRltProblemCategories.Any(y => input.ProblemCategoryIds.Contains(y.ProblemCategoryId)))
                .OrderBy(x => x.Order);

            var result = new PagedResultDto<ProblemDto>()
            {
                TotalCount = problems.Count(),
                Items = input.IsAll
                ? ObjectMapper.Map<List<Entities.Problem>, List<ProblemDto>>(problems.ToList())
                : ObjectMapper.Map<List<Entities.Problem>, List<ProblemDto>>(problems.Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProblemDto> Create(ProblemCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("名称不能为空");

            CheckSameName(input.Name, null);//名称验证

            var problem = new Entities.Problem(_guidGenerator.Create())
            {
                Name = input.Name,
                Order = input.Order,
                Content = input.Content,
                ProblemRltProblemCategories = new List<ProblemRltProblemCategory>(),
            };

            // 重新保存关联问题分类信息
            foreach (var category in input.ProblemRltProblemCategories)
            {
                problem.ProblemRltProblemCategories.Add(new ProblemRltProblemCategory(_guidGenerator.Create())
                {
                    ProblemCategoryId = category.ProblemCategoryId,
                    ProblemId = problem.Id,
                });
            }

            await _repository.InsertAsync(problem);

            return ObjectMapper.Map<Entities.Problem, ProblemDto>(problem);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProblemDto> Update(ProblemUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("名称不能为空");

            CheckSameName(input.Name, input.Id);//名称验证

            var problem = await _repository.GetAsync(input.Id);
            if (problem == null)
            {
                throw new UserFriendlyException("该问题不存在");
            }

            problem.Name = input.Name;
            problem.Content = input.Content;
            problem.Order = input.Order;

            // 删除之前关联信息
            await _ProblemRltProblemCategoryRepository.DeleteAsync(x => x.ProblemId == problem.Id);

            problem.ProblemRltProblemCategories = new List<ProblemRltProblemCategory>();
            // 重新保存关联问题分类信息
            foreach (var category in input.ProblemRltProblemCategories)
            {
                problem.ProblemRltProblemCategories.Add(new ProblemRltProblemCategory(_guidGenerator.Create())
                {
                    ProblemCategoryId = category.ProblemCategoryId,
                    ProblemId = problem.Id,
                });
            }


            await _repository.UpdateAsync(problem);

            return ObjectMapper.Map<Entities.Problem, ProblemDto>(problem);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// 私有方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckSameName(string name, Guid? id)
        {
            var sameProblems = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());

            if (id.HasValue)
            {
                sameProblems = sameProblems.Where(o => o.Id != id.Value);
            }

            if (sameProblems.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("该问题名称已存在！");
            }

            return true;
        }
    }
}
