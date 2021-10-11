using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Authorization;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicRepairTestItemAppService : StdBasicAppService, IStdBasicRepairTestItemAppService
    {
        private IRepository<RepairTestItem, Guid> _repairTestItemRepository;
        private IGuidGenerator _generator;

        public StdBasicRepairTestItemAppService(
            IGuidGenerator generator,
            IRepository<RepairTestItem, Guid> repairTestItemRepository
           )
        {
            _generator = generator;
            _repairTestItemRepository = repairTestItemRepository;
        }

        public Task<RepairTestItemDto> get(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new UserFriendlyException("Id 不能为空");
            }

            var ent = _repairTestItemRepository.WithDetails(x => x.File).Where(x => x.Id == id).FirstOrDefault();
            return Task.FromResult(ObjectMapper.Map<RepairTestItem, RepairTestItemDto>(ent));
        }

        [Authorize(StdBasicPermissions.RepairTestItem.Create)]
        public async Task<RepairTestItemDto> Create(RepairTestItemCreateDto input)
        {
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (_repairTestItemRepository.FirstOrDefault(x => x.RepairItemId == input.RepairItemId && x.Name == input.Name) != null)
                throw new UserFriendlyException("该维修项中已存在相同名称的测试项");
            var ent = new RepairTestItem(_generator.Create());
            ent.Name = input.Name;
            ent.Type = input.Type;
            ent.Unit = input.Unit;
            ent.RepairItemId = input.RepairItemId;
            ent.DefaultValue = input.DefaultValue;
            ent.MaxRated = input.MaxRated;
            ent.MinRated = input.MinRated;
            ent.FileId = input.FileId;
            ent.Order = input.Order;
            await _repairTestItemRepository.InsertAsync(ent);

            return ObjectMapper.Map<RepairTestItem, RepairTestItemDto>(ent);
        }


        [Authorize(StdBasicPermissions.RepairTestItem.Update)]
        public async Task<bool> Update(RepairTestItemUpdateDto input)
        {
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (_repairTestItemRepository.FirstOrDefault(x => x.RepairItemId == input.RepairItemId && x.Name == input.Name && x.Id != input.Id) != null)
                throw new UserFriendlyException("该维修项中已存在相同名称的测试项");
            var ent = new RepairTestItem(input.Id);
            ent.Name = input.Name;
            ent.Type = input.Type;
            ent.Unit = input.Unit;
            ent.RepairItemId = input.RepairItemId;
            ent.DefaultValue = input.DefaultValue;
            ent.MaxRated = input.MaxRated;
            ent.MinRated = input.MinRated;
            ent.Order = input.Order;
            ent.FileId = input.FileId;

            await _repairTestItemRepository.UpdateAsync(ent);
            return true;
        }
        [Authorize(StdBasicPermissions.RepairTestItem.UpdateOrder)]
        public async Task<bool> UpdateOrder(RepairTestItemSimpleDto input)
        {
            var ent = await _repairTestItemRepository.GetAsync(input.Id);
            ent.Order = input.Order;
            await _repairTestItemRepository.UpdateAsync(ent);
            return true;
        }

        [Authorize(StdBasicPermissions.RepairTestItem.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            await _repairTestItemRepository.DeleteAsync(id);
            return true;
        }


    }
}
