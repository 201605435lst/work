using Microsoft.AspNetCore.Authorization;
using SnAbp.Emerg.Authorization;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Entities;
using SnAbp.Emerg.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Emerg.Services
{
    [Authorize]
    public class EmergPlanAppService : EmergAppService, IEmergPlanAppService
    {
        private readonly IRepository<EmergPlan, Guid> _repository;
        private readonly IRepository<EmergPlanRltComponentCategory, Guid> _repositoryEmergPlanRltComponentCategory;
        private readonly IRepository<EmergPlanRltFile, Guid> _repositoryEmergPlanRltFile;
        private readonly IGuidGenerator _guidGenerator;
        private IDataFilter DataFilter { get; }
        public EmergPlanAppService(
            IRepository<EmergPlan, Guid> repository,
            IRepository<EmergPlanRltComponentCategory, Guid> repositoryEmergPlanRltComponentCategory,
            IRepository<EmergPlanRltFile, Guid> repositoryEmergPlanRltFile,
            IGuidGenerator guidGenerator,
            IDataFilter dataFilter
            )
        {
            _repository = repository;
            _repositoryEmergPlanRltComponentCategory = repositoryEmergPlanRltComponentCategory;
            _repositoryEmergPlanRltFile = repositoryEmergPlanRltFile;
            _guidGenerator = guidGenerator;
            DataFilter = dataFilter;
        }

        /// <summary>
        /// 创建应急预案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(EmergPermissions.Plan.Create)]
        public async Task<EmergPlanDto> Create(EmergPlanCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("名称不能为空");
            if (string.IsNullOrEmpty(input.Summary)) throw new UserFriendlyException("预案摘要不能为空");
            if (input.LevelId == null || Guid.Empty == input.LevelId) throw new UserFriendlyException("预案等级不能为空");
            if (input.ComponentCategoryIds.Count == 0 || input.ComponentCategoryIds == null) throw new UserFriendlyException("构件不能为空");

            checkSameName(input.Name, null);

            var emergPlanId = _guidGenerator.Create();
            var emergPlan = new EmergPlan(emergPlanId);

            emergPlan.Name = input.Name; //名称
            emergPlan.LevelId = input.LevelId;//预案等级
            emergPlan.Summary = input.Summary;//预案摘要
            emergPlan.Remark = input.Remark; //备注
            emergPlan.Flow = input.Flow;//预案流程
            emergPlan.Content = input.Content;//预案图文资料

            //保存预案-构件关联表
            emergPlan.EmergPlanRltComponentCategories = new List<EmergPlanRltComponentCategory>();

            foreach (var componentCategoryId in input.ComponentCategoryIds)
            {
                emergPlan.EmergPlanRltComponentCategories.Add(new EmergPlanRltComponentCategory(Guid.NewGuid())
                {
                    ComponentCategoryId = componentCategoryId
                });
            }

            //保存预案-文件关联表
            emergPlan.EmergPlanRltFiles = new List<EmergPlanRltFile>();

            foreach (var fileId in input.FileIds)
            {
                emergPlan.EmergPlanRltFiles.Add(new EmergPlanRltFile(Guid.NewGuid())
                {
                    FileId = fileId
                });
            }

            await _repository.InsertAsync(emergPlan);
            return ObjectMapper.Map<EmergPlan, EmergPlanDto>(emergPlan);

        }

        /// <summary>
        /// 删除应急预案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(EmergPermissions.Plan.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (null == id || id == Guid.Empty) throw new UserFriendlyException("id 不能为空");
            await _repository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// 查询应急预案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(EmergPermissions.Plan.Detail)]
        public Task<EmergPlanDto> Get(Guid id)
        {
            if (id == null || Guid.Empty == id) throw new UserFriendlyException("id不能为空");
            EmergPlan emergPlan;
            //using (DataFilter.Disable<ISoftDelete>())
            //{
            emergPlan = _repository.WithDetails().FirstOrDefault(o => o.Id == id);
            if (emergPlan != null && emergPlan.Level.IsDeleted)
            {
                emergPlan.Level = null;
                //}
            }
            return Task.FromResult(ObjectMapper.Map<EmergPlan, EmergPlanDto>(emergPlan));
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>*
        /// <returns></returns>
        public Task<PagedResultDto<EmergPlanDto>> GetList(EmergPlanSearchDto input)
        {
            var result = new PagedResultDto<EmergPlanDto>();
            //using (DataFilter.Disable<ISoftDelete>())//查询软删除的数据
            //{
            var emergPlan = _repository.WithDetails()
            .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Summary.Contains(input.Keywords))
            .WhereIf(input.ComponentCategoryIds != null && input.ComponentCategoryIds.Any(), x => x.EmergPlanRltComponentCategories.Any(y => input.ComponentCategoryIds.Contains(y.ComponentCategoryId)))
            .WhereIf(Guid.Empty != input.LevelId, x => x.LevelId == input.LevelId);
            result.TotalCount = emergPlan.Count();
            result.Items = ObjectMapper.Map<List<EmergPlan>, List<EmergPlanDto>>(emergPlan.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            //foreach (var emergPlanDto in result.Items)
            //{
            //    if (emergPlanDto.Level.IsDeleted)
            //    {
            //        emergPlanDto.Level = null;
            //    }
            //}
            //}
            return Task.FromResult(result);
        }

        /// <summary>
        /// 更新预案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(EmergPermissions.Plan.Update)]
        public async Task<EmergPlanDto> Update(EmergPlanUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("名称不能为空");
            if (string.IsNullOrEmpty(input.Summary)) throw new UserFriendlyException("预案摘要不能为空");
            if (input.LevelId == null || Guid.Empty == input.LevelId) throw new UserFriendlyException("预案等级不能为空");
            if (input.ComponentCategoryIds.Count == 0 || input.ComponentCategoryIds == null) throw new UserFriendlyException("构件不能为空");

            checkSameName(input.Name, input.Id);

            var emergPlan = await _repository.GetAsync(input.Id);
            if (null == emergPlan)
            {
                throw new UserFriendlyException("应急预案不存在");
            }
            emergPlan.Name = input.Name; //名称
            emergPlan.LevelId = input.LevelId;//预案等级
            emergPlan.Summary = input.Summary;//预案摘要
            emergPlan.Remark = input.Remark; //备注
            emergPlan.Flow = input.Flow;//预案流程
            emergPlan.Content = input.Content;//预案图文资料

            //清除预案-构件关联表信息
            await _repositoryEmergPlanRltComponentCategory.DeleteAsync(x => x.EmergPlanId == emergPlan.Id);
            //重新保存预案-构件关联表信息
            emergPlan.EmergPlanRltComponentCategories = new List<EmergPlanRltComponentCategory>();
            foreach (var componentCategoryId in input.ComponentCategoryIds)
            {
                emergPlan.EmergPlanRltComponentCategories.Add(new EmergPlanRltComponentCategory(Guid.NewGuid())
                {
                    ComponentCategoryId = componentCategoryId
                });
            }

            //清除预案-文件关联表信息
            await _repositoryEmergPlanRltFile.DeleteAsync(x => x.EmergPlanId == emergPlan.Id);
            //重新保存预案-文件关联表信息
            foreach (var fileId in input.FileIds)
            {
                emergPlan.EmergPlanRltFiles.Add(new EmergPlanRltFile(Guid.NewGuid())
                {
                    FileId = fileId
                });
            }

            await _repository.UpdateAsync(emergPlan);
            return ObjectMapper.Map<EmergPlan, EmergPlanDto>(emergPlan);
        }

        //验证应急预案名称是否唯一
        private bool checkSameName(string name, Guid? id)
        {
            var sameName = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());
            if (id.HasValue)
            {
                sameName = sameName.Where(o => o.Id != id.Value);
            }
            if (sameName.Count() > 0)
            {
                throw new UserFriendlyException("该名称已存在");
            }
            return true;

        }
    }
}
