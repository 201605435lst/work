using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.StdBasic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.StdBasic.Dtos.Repair.RepairItem;
using SnAbp.StdBasic.Enums;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using SnAbp.Identity;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicRepairItemAppService : StdBasicAppService, IStdBasicRepairItemAppService
    {
        private IGuidGenerator _generator;
        private IRepairItemRepository _repairItemRepository;
        private IRepository<RepairItemRltComponentCategory, Guid> _repairItemRltComponentCategoryRepository;
        private IRepository<RepairTestItem, Guid> _repairTestItemRepository;
        private readonly IRepository<RepairItemRltOrganizationType, Guid> _repairItemRltOrganizationType;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;

        public StdBasicRepairItemAppService(
            IGuidGenerator generator,
            IRepairItemRepository repairItemRepository,
            IRepository<RepairItemRltComponentCategory, Guid> repairItemRltComponentCategoryRepository,
            IRepository<RepairTestItem, Guid> repairTestItemRepository,
            IRepository<RepairItemRltOrganizationType, Guid> repairItemRltOrganizationType,
            IRepository<DataDictionary, Guid> dataDictionaries
            )
        {
            _generator = generator;
            _repairItemRepository = repairItemRepository;
            _repairItemRltComponentCategoryRepository = repairItemRltComponentCategoryRepository;
            _repairTestItemRepository = repairTestItemRepository;
            _repairItemRltOrganizationType = repairItemRltOrganizationType;
            _dataDictionaries = dataDictionaries;
        }

        public async Task<RepairItemDetailDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不正确");
            }

            var result = new RepairItemDetailDto();
            try
            {
                await Task.Run(() =>
                {
                    var ent = _repairItemRepository.WithDetails().FirstOrDefault(s => s.Id == id);
                    if (ent == null)
                    {
                        throw new UserFriendlyException("该维修项不存在");
                    }

                    result = ObjectMapper.Map<RepairItem, RepairItemDetailDto>(ent);
                });

                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public async Task<PagedResultDto<RepairItemDto>> GetList(RepairItemSearchDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            var orizationTypeList = new List<Guid>();
            if (input.ExecutiveUnitId != null && input.ExecutiveUnitId != Guid.Empty)
            {
                orizationTypeList = _repairItemRltOrganizationType
                    .Where(x => x.OrganizationTypeId == input.ExecutiveUnitId)
                    .Select(x => x.RepairItemId).ToList();
            }

            var list = _repairItemRepository.WithDetails()
                .WhereIf(input.IsMonth != null, x => x.IsMonth == input.IsMonth)
                .WhereIf(input.ComponentCategoryIds != null && input.ComponentCategoryIds.Count > 0,
                    x => x.RepairItemRltComponentCategories.Any(rlt => input.ComponentCategoryIds.Contains(rlt.ComponentCategoryId))
                )
                .WhereIf(input.TopGroupId != null && input.TopGroupId != Guid.Empty, x => x.Group.ParentId == input.TopGroupId)
                .WhereIf(input.GroupId != null && input.GroupId != Guid.Empty, x => x.GroupId == input.GroupId)
                .WhereIf(input.Type != null, x => x.Type == input.Type)
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords),
                    x => x.Content.Contains(input.KeyWords) ||
                    x.Remark.Contains(input.KeyWords) ||
                    x.Unit.Contains(input.KeyWords))
                .WhereIf(RepairTagId != Guid.Empty, x => x.TagId == RepairTagId)
                .WhereIf(orizationTypeList.Count > 0, x => orizationTypeList.Contains(x.Id))
                .OrderBy(x => x.IsMonth).ThenBy(y => y.Group.Parent.Name).ThenBy(z => z.Group.Name).ThenBy(m => m.Number);

            var result = new PagedResultDto<RepairItemDto>()
            {
                TotalCount = list.Count(),
                Items = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
            };

            foreach (var item in result.Items)
            {
                item.RepairItemRltOrganizationTypes = item.RepairItemRltOrganizationTypes.OrderBy(x => x.OrganizationType.Name).ToList();
            }

            return result;
        }

        /// <summary>
        /// 获取最大编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<int> GetMaxNumber(GetMaxNumberDto input)
        {
            if (input.GroupId == null || input.GroupId == Guid.Empty) throw new UserFriendlyException("请选择设备");

            if (input.IsMonth == null) throw new UserFriendlyException("请选择年月类型");

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var mRepairItems = _repairItemRepository.Where(x => x.TagId == RepairTagId && x.IsMonth == input.IsMonth && x.GroupId == input.GroupId).OrderByDescending(s => s.Number).ToList();

            var number = mRepairItems.Count > 0 ? mRepairItems.First().Number : 0;
            return number;
        }

        [Authorize(StdBasicPermissions.RepairItem.Create)]
        public async Task<RepairItemDto> Create(RepairItemCreateDto input)
        {
            var dto = ObjectMapper.Map<RepairItemCreateDto, RepairItemDto>(input);
            dto.Id = _generator.Create();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            var ent = ObjectMapper.Map<RepairItemDto, RepairItem>(dto);
            ent.TagId = RepairTagId;
            ent.RepairItemRltComponentCategories = new List<RepairItemRltComponentCategory>();
            foreach (var categoryId in input.ComponentCategoryIds)
            {
                ent.RepairItemRltComponentCategories.Add(new RepairItemRltComponentCategory(_generator.Create())
                {
                    RepairItemId = ent.Id,
                    ComponentCategoryId = categoryId
                });
            }
            //添加执行单位关联表
            ent.RepairItemRltOrganizationTypes = new List<RepairItemRltOrganizationType>();
            foreach (var organizationTypeIds in input.OrganizationTypeIds)
            {
                ent.RepairItemRltOrganizationTypes.Add(new RepairItemRltOrganizationType(_generator.Create())
                {
                    RepairItemId = ent.Id,
                    OrganizationTypeId = organizationTypeIds
                });
            }

            // 判断该维修分组下当前编号是否存在
            var existNumberEntity = _repairItemRepository.FirstOrDefault(x => x.TagId == RepairTagId && x.IsMonth == input.IsMonth && x.GroupId == input.GroupId && x.Number == input.Number);
            if (existNumberEntity != null)
            {
                var repairItems = _repairItemRepository.Where(x => x.TagId == RepairTagId && x.IsMonth == input.IsMonth && x.GroupId == input.GroupId && x.Number >= input.Number).OrderBy(s => s.Number).ToList();
                foreach (var item in repairItems)
                {
                    item.Number = ++input.Number;
                }
            }

            await _repairItemRepository.InsertAsync(ent);

            return ObjectMapper.Map<RepairItem, RepairItemDto>(ent);
        }
        /// <summary>
        /// 标签迁移
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        [Authorize(StdBasicPermissions.RepairItem.CreateTagMigration)]
        public async Task<bool> CreateTagMigration(RepairItemTagMigratioDto input)
        {
            // 1、判断需要迁移的设备类型不能为空
            if (input.RepairGroupIds.Count == 0) throw new UserFriendlyException("请选择设备类型");
            // 源数据id
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            // 2、获得迁移的源数据
            var oldRepairItems = _repairItemRepository
                .WithDetails()
                .Where(x => input.RepairGroupIds.Contains(x.GroupId) && x.TagId == RepairTagId);
            // 3、删除迁移目标数据
            // 3.1 获得目标数据GroupId
            var removeRepairItems = oldRepairItems.Select(x => x.GroupId).ToList();
            // 3.2 根据groupId和TargetId删除数据
            await _repairItemRepository.DeleteAsync(x => removeRepairItems.Contains(x.GroupId) && x.TagId == input.TargetTagId);
            var newRepairItems = new List<RepairItem>();

            foreach (var oldRepairItem in oldRepairItems)
            {
                var repairItem = new RepairItem(_generator.Create())
                {
                    Number = oldRepairItem.Number,
                    GroupId = oldRepairItem.GroupId,
                    Type = oldRepairItem.Type,
                    Content = oldRepairItem.Content,
                    Unit = oldRepairItem.Unit,
                    Period = oldRepairItem.Period,
                    PeriodUnit = oldRepairItem.PeriodUnit,
                    IsMonth = oldRepairItem.IsMonth,
                    Remark = oldRepairItem.Remark,
                    TagId = input.TargetTagId,
                    RepairTestItems = new List<RepairTestItem>(),
                    RepairItemRltComponentCategories = new List<RepairItemRltComponentCategory>(),
                    RepairItemRltOrganizationTypes = new List<RepairItemRltOrganizationType>(),
                };

                // 添加关联分类
                foreach (var oldRlt in oldRepairItem.RepairItemRltComponentCategories)
                {
                    repairItem.RepairItemRltComponentCategories.Add(
                        new RepairItemRltComponentCategory(_generator.Create())
                        {
                            ComponentCategoryId = oldRlt.ComponentCategoryId
                        }
                    );
                }
                foreach (var oldRltOrganization in oldRepairItem.RepairItemRltOrganizationTypes)
                {
                    repairItem.RepairItemRltOrganizationTypes.Add(
                        new RepairItemRltOrganizationType(_generator.Create())
                        {
                            OrganizationTypeId = oldRltOrganization.OrganizationTypeId,
                        }
                        );
                }
                foreach (var oldRltTest in oldRepairItem.RepairTestItems)
                {
                    repairItem.RepairTestItems.Add(
                        new RepairTestItem(_generator.Create())
                        {
                            Name = oldRltTest.Name,
                            Type = oldRltTest.Type,
                            Unit = oldRltTest.Unit,
                            DefaultValue = oldRltTest.DefaultValue,
                            MaxRated = oldRltTest.MaxRated,
                            MinRated = oldRltTest.MinRated,
                            FileId = oldRltTest.FileId,
                        }
                        );
                }
                newRepairItems.Add(repairItem);
            }
            await _repairItemRepository.InsertRange(newRepairItems);

            return true;
        }
        [Authorize(StdBasicPermissions.RepairItem.Update)]
        public async Task<bool> Update(RepairItemUpdateDto input)
        {
            var ent = await _repairItemRepository.GetAsync(input.Id);
            if (ent == null) throw new UserFriendlyException("该维修项不存在");


            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            // 删除已经存在的构件分类关联关系
            await _repairItemRltComponentCategoryRepository.DeleteAsync(x => x.RepairItemId == ent.Id);

            foreach (var categoryId in input.ComponentCategoryIds)
            {
                await _repairItemRltComponentCategoryRepository.InsertAsync(new RepairItemRltComponentCategory(_generator.Create())
                {
                    RepairItemId = ent.Id,
                    ComponentCategoryId = categoryId
                });
            }

            // 删除执行单位关联关系
            await _repairItemRltOrganizationType.DeleteAsync(x => x.RepairItemId == ent.Id);

            //重新保存执行单位关联关系
            foreach (var inputOrganizationTypeId in input.OrganizationTypeIds)
            {
                await _repairItemRltOrganizationType.InsertAsync(new RepairItemRltOrganizationType(_generator.Create())
                {
                    RepairItemId = ent.Id,
                    OrganizationTypeId = inputOrganizationTypeId
                });
            }

            ent.TagId = RepairTagId;
            ent.IsMonth = input.IsMonth;
            ent.GroupId = input.GroupId;
            ent.Type = input.Type;
            ent.Number = input.Number;
            ent.Content = input.Content;
            ent.Unit = input.Unit;
            ent.Period = input.Period;
            ent.PeriodUnit = input.PeriodUnit;
            ent.Remark = input.Remark;

            // 判断该维修分组下当前编号是否存在
            var existNumberEntity = _repairItemRepository.FirstOrDefault(x =>
                x.TagId == RepairTagId &&
                x.IsMonth == ent.IsMonth &&
                x.GroupId == ent.GroupId &&
                x.Number == ent.Number &&
                x.Id != ent.Id);

            if (existNumberEntity != null)
            {
                var repairItems = _repairItemRepository
                    .Where(x =>
                        x.TagId == RepairTagId &&
                        x.IsMonth == ent.IsMonth &&
                        x.GroupId == ent.GroupId &&
                        x.Id != ent.Id &&
                        x.Number >= ent.Number)
                    .OrderBy(x => x.Number)
                    .ToList();

                var startIndex = ent.Number;
                foreach (var item in repairItems)
                {
                    item.Number = ++startIndex;
                }
            }

            //await _repairItemRepository.UpdateAsync(ent);
            await CurrentUnitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 批量更新执行单位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateOrganizationType(RepairItemUpdateSimpleDto input)
        {
            var repairItem = _repairItemRepository
                .WhereIf(input.RepairItemIds.Count > 0, x => input.RepairItemIds.Contains(x.Id)).ToList();

            if (!repairItem.Any())
            {
                throw new UserFriendlyException("维修项不存在");
            }
            foreach (var item in repairItem)
            {
                //删除之前相关的执行单位
                await _repairItemRltOrganizationType.DeleteAsync(x => x.RepairItemId == item.Id);
                //重新关联
                item.RepairItemRltOrganizationTypes = new List<RepairItemRltOrganizationType>();
                foreach (var inputOrganizationTypeId in input.OrganizationTypeIds)
                {
                    item.RepairItemRltOrganizationTypes.Add(new RepairItemRltOrganizationType(_generator.Create())
                    {
                        RepairItemId = item.Id,
                        OrganizationTypeId = inputOrganizationTypeId
                    });
                }

            }
            return true;
        }

        [Authorize(StdBasicPermissions.RepairItem.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _repairItemRepository.DeleteAsync(id);

            return true;
        }

    }
}
