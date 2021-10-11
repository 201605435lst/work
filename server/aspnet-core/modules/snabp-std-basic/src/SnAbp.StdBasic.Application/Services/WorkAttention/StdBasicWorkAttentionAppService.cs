using Microsoft.AspNetCore.Authorization;
using SnAbp.Identity;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicWorkAttentionAppService : StdBasicAppService, IStdBasicWorkAttentionAppService
    {
        private readonly IRepository<WorkAttention, Guid> _workAttentionsRepository;
        private readonly IGuidGenerator _generateGuid;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;

        public StdBasicWorkAttentionAppService(
           IRepository<WorkAttention, Guid> workAttentionsRepository,
           IGuidGenerator generateGuid,
           IRepository<DataDictionary, Guid> dataDictionaries
            )
        {
            _workAttentionsRepository = workAttentionsRepository;
            _generateGuid = generateGuid;
            _dataDictionaries = dataDictionaries;
        }
        /// <summary>
        /// 获取单个事项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<WorkAttentionDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的事项");
            var workAttention = _workAttentionsRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (workAttention == null) throw new UserFriendlyException("当前事项不存在");
            var result = ObjectMapper.Map<WorkAttention, WorkAttentionDto>(workAttention);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WorkAttentionDto>> GetList(WorkAttentionSearchDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var result = new PagedResultDto<WorkAttentionDto>();
            var workAttention = _workAttentionsRepository.WithDetails()
                .WhereIf(input.IsType, x => x.IsType)
                 .WhereIf(!input.IsType, x => !x.IsType)
                  .WhereIf(RepairTagId != Guid.Empty, x => x.RepairTagId == RepairTagId)
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Content.Contains(input.KeyWords))
                .WhereIf(input.TypeId != null && input.TypeId != Guid.Empty, x => x.TypeId == input.TypeId).ToList();
            result.Items = input.IsAll ? ObjectMapper.Map<List<WorkAttention>, List<WorkAttentionDto>>(workAttention.OrderBy(x => x.Content).ToList()) : ObjectMapper.Map<List<WorkAttention>, List<WorkAttentionDto>>(workAttention.OrderBy(x => x.Content).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            result.TotalCount = workAttention.Count();
            return result;
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.WorkAttention.Create)]
        public async Task<WorkAttentionDto> Create(WorkAttentionCreateDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            if (string.IsNullOrEmpty(input.Content)) throw new UserFriendlyException("注意事项无内容");
            if (!StringUtil.CheckSpaceValidity(input.Content))
            {
                throw new UserFriendlyException("注意事项内容不能包含空格");
            }
           
            CheckSameType(input.Content, null, input.IsType, RepairTagId, input.TypeId);

            var workAttention = new WorkAttention(_generateGuid.Create());
            workAttention.TypeId = input.TypeId;
            workAttention.Content = input.Content;
            workAttention.IsType = input.IsType;
            workAttention.RepairTagId = RepairTagId;
            var resEnt = await _workAttentionsRepository.InsertAsync(workAttention);
            return ObjectMapper.Map<WorkAttention, WorkAttentionDto>(resEnt);
        }



        [Authorize(StdBasicPermissions.WorkAttention.Update)]
        public async Task<WorkAttentionDto> Update(WorkAttentionUpdateDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            if (string.IsNullOrEmpty(input.Content)) throw new UserFriendlyException("注意事项无内容");
            if (!StringUtil.CheckSpaceValidity(input.Content))
            {
                throw new UserFriendlyException("注意事项内容不能包含空格");
            }
            CheckSameType(input.Content, input.Id, input.IsType, RepairTagId, input.TypeId);

            var workAttention = await _workAttentionsRepository.GetAsync(input.Id);
            input.RepairTagId = RepairTagId;
            ObjectMapper.Map<WorkAttentionUpdateDto, WorkAttention>(input, workAttention);
            await _workAttentionsRepository.UpdateAsync(workAttention);
            return ObjectMapper.Map<WorkAttention, WorkAttentionDto>(workAttention);
        }
        [Authorize(StdBasicPermissions.WorkAttention.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var workAttention = _workAttentionsRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (workAttention == null) throw new UserFriendlyException("该项不存在");
            if (workAttention.IsType)
            {
                await _workAttentionsRepository.DeleteAsync(x => x.TypeId == id);
            }
            await _workAttentionsRepository.DeleteAsync(id);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">类别或者注意事项</param>
        /// <param name="Id"></param>
        /// <param name="isType">是否类别</param>
        /// <param name="RepairTagId">维修项类别</param>
        /// <param name="TypeId">分类id</param>
        /// <returns></returns>
        private bool CheckSameType(string content, Guid? Id, bool isType, Guid? RepairTagId, Guid? TypeId)
        {
            var workAttention = new List<WorkAttention>();
            if (isType)
            {
                workAttention = _workAttentionsRepository.Where(x => x.Content == content && x.IsType && x.RepairTagId == RepairTagId)
                    .WhereIf(Id.HasValue, x => x.Id != Id).ToList();
                if (workAttention.Count() > 0)
                {
                    throw new Volo.Abp.UserFriendlyException("当前类别名称已存在！！！");
                }
            }
            else
            {
                workAttention = _workAttentionsRepository.Where(x => x.Content == content && !x.IsType && x.RepairTagId == RepairTagId && x.TypeId == TypeId)
                     .WhereIf(Id.HasValue, x => x.Id != Id).ToList();
                if (workAttention.Count() > 0)
                {
                    throw new Volo.Abp.UserFriendlyException("当前注意事项已存在！！！");
                }
            }
            return true;
        }
    }
}
