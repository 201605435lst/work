using Microsoft.AspNetCore.Authorization;
using SnAbp.Identity;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices.InfluenceRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicInfluenceRangeAppService : StdBasicAppService, IStdBasicInfluenceRangeAppService
    {
        private readonly IRepository<InfluenceRange, Guid> _influenceRanges;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;

        public StdBasicInfluenceRangeAppService(IRepository<InfluenceRange, Guid> influenceRanges,
            IRepository<DataDictionary, Guid> dataDictionaries)
        {
            _influenceRanges = influenceRanges;
            _dataDictionaries = dataDictionaries;
        }

        public async Task<InfluenceRangeDto> Get(Guid id)
        {
            var resEnt = await _influenceRanges.GetAsync(id);
            var res = ObjectMapper.Map<InfluenceRange, InfluenceRangeDto>(resEnt);
            return res;
        }

        public async Task<PagedResultDto<InfluenceRangeDto>> GetList(InfluenceRangeSearchDto input)
        {
            PagedResultDto<InfluenceRangeDto> res = new PagedResultDto<InfluenceRangeDto>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var allEnt = _influenceRanges.Where(s => s.TagId == RepairTagId &&
              (input.RepairLevel == null || input.RepairLevel.Contains(s.RepairLevel)) &&
              (string.IsNullOrEmpty(input.Keyword) || s.Content.Contains(input.Keyword)));
            if (input.IsAll == false)
            {
                res.TotalCount = allEnt.Count();
                res.Items = ObjectMapper.Map<List<InfluenceRange>, List<InfluenceRangeDto>>(allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            else
            {
                res.Items = ObjectMapper.Map<List<InfluenceRange>, List<InfluenceRangeDto>>(allEnt.ToList());
                res.TotalCount = res.Items.Count;
            }
            return res;
        }

        [Authorize(StdBasicPermissions.InfluenceRange.Create)]
        public async Task<InfluenceRangeDto> Create(InfluenceRangeCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Content)) throw new UserFriendlyException("影响范围无内容");
            if (!StringUtil.CheckSpaceValidity(input.Content))
            {
                throw new UserFriendlyException("影响范围内容不能包含空格");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            if (_influenceRanges.FirstOrDefault(x => x.RepairLevel == input.RepairLevel && x.Content == input.Content && x.TagId == RepairTagId) != null)
                throw new UserFriendlyException("当前维修级别下的影响范围内容已存在");
            InfluenceRange ent = new InfluenceRange(Guid.NewGuid());
            ent.Content = input.Content;
            ent.LastModifyTime = DateTime.Now;
            ent.RepairLevel = input.RepairLevel;
            ent.TagId = RepairTagId;
            var resEnt = await _influenceRanges.InsertAsync(ent);
            return ObjectMapper.Map<InfluenceRange, InfluenceRangeDto>(resEnt);
        }

        [Authorize(StdBasicPermissions.InfluenceRange.Update)]
        public async Task<InfluenceRangeDto> Update(InfluenceRangeUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            if (!StringUtil.CheckSpaceValidity(input.Content))
            {
                throw new UserFriendlyException("影响范围内容不能包含空格");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var ent = _influenceRanges.FirstOrDefault(s => s.TagId == RepairTagId && input.Id == s.Id);
            if (ent == null) throw new UserFriendlyException("此影响范围不存在");
            if (_influenceRanges.FirstOrDefault(x => x.RepairLevel == input.RepairLevel &&
                      x.Content == input.Content && x.TagId == RepairTagId && x.Id != input.Id) != null)
                throw new UserFriendlyException("当前维修级别下的影响范围内容已存在");
            ent.Content = input.Content;
            ent.RepairLevel = input.RepairLevel;
            ent.LastModifyTime = DateTime.Now;
            ent.TagId = RepairTagId;
            var resEnt = await _influenceRanges.UpdateAsync(ent);
            return ObjectMapper.Map<InfluenceRange, InfluenceRangeDto>(resEnt);
        }

        [Authorize(StdBasicPermissions.InfluenceRange.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _influenceRanges.FirstOrDefault(s => id == s.Id);
            if (ent == null) throw new UserFriendlyException("此影响范围不存在");
            await _influenceRanges.DeleteAsync(id);
            return true;
        }
    }
}
