using Microsoft.AspNetCore.Authorization;
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
    public class StdBasicRepairGroupAppService : StdBasicAppService, IStdBasicRepairGroupAppService
    {
        private readonly IRepository<RepairGroup, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;

        public StdBasicRepairGroupAppService(IRepository<RepairGroup, Guid> repository, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }



        public PagedResultDto<RepairGroupSimpleDto> GetTreeList(RepairGroupGetListDto input)
        {
            var repairGroup = new List<RepairGroup>();
            var result = new PagedResultDto<RepairGroupSimpleDto>();
            if (input.TreeSelect)
            {
                var list = _repository.ToList();
              //转成树状数组
                repairGroup = GuidKeyTreeHelper<RepairGroup>.GetTree(list);

            }
            else
            {
                repairGroup = _repository
                              .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                              .WhereIf(input.ParentId == null, x => x.ParentId == null).OrderBy(x => x.Order).ToList();
            }


            result.TotalCount = repairGroup.Count();
            result.Items = ObjectMapper.Map<List<RepairGroup>, List<RepairGroupSimpleDto>>(repairGroup);
            var standardList = repairGroup.OrderBy(x => x.Order).ToList();
            foreach (var list in standardList)
            {
                list.Order = standardList.IndexOf(list) + 1;
            }
            result.Items = ObjectMapper.Map<List<RepairGroup>, List<RepairGroupSimpleDto>>(standardList);
            return result;
        }

        [Authorize(StdBasicPermissions.RepairGroup.Create)]
        public async Task<RepairGroupDto> Create(RepairGroupCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            var repairGroup = new RepairGroup(_guidGenerator.Create());
            saveCheckSameName(input.Name, null, input.ParentId);

            var repairGroups = _repository.WithDetails().Where(x => x.ParentId == input.ParentId);
            if (repairGroups.Any(x => x.Order == input.Order))
            {
                var repairGroupsAstrict = _repository.WithDetails().Where(x => x.Order >= input.Order);
                repairGroup.Order = input.Order;
                foreach (var repairsAstrict in repairGroupsAstrict)
                {
                    repairsAstrict.Order += 1;
                }
            }
            else
            {
                repairGroup.Order = repairGroups.Count() + 1;
            }
            repairGroup.Name = input.Name;
            repairGroup.ParentId = input.ParentId;
            repairGroup.Remark = input.Remark;
            var result = await _repository.InsertAsync(repairGroup);
            return ObjectMapper.Map<RepairGroup, RepairGroupDto>(result);
        }
       
        [Authorize(StdBasicPermissions.RepairGroup.Update)]
        public async Task<bool> Update(RepairGroupUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入分组的id");
            var repairGroup = await _repository.GetAsync(input.Id);
            if (repairGroup == null) throw new UserFriendlyException("该分组不存在");
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("请输入分组的名称");
            saveCheckSameName(input.Name, input.Id, input.ParentId);

            var repairGroups = _repository.WithDetails().Where(x => x.ParentId == input.ParentId);
            /*判断>=1和<=-1就行*/
            if (input.Order - repairGroup.Order >= 1)
            {
                if (input.Order <= repairGroups.Count())
                {
                    var repairGroupsAstrict = _repository.WithDetails().Where(x => x.ParentId == input.ParentId && x.Order > repairGroup.Order && x.Order <= input.Order);

                    foreach (var repairsAstrict in repairGroupsAstrict)
                    {
                        repairsAstrict.Order -= 1;
                    }
                    repairGroup.Order = input.Order;
                }
                else
                {
                    var repairGroupsAstrict = _repository.WithDetails().Where(x => x.ParentId == input.ParentId && x.Order > repairGroup.Order && x.Order <= input.Order);

                    foreach (var repairsAstrict in repairGroupsAstrict)
                    {
                        repairsAstrict.Order -= 1;
                    }
                    repairGroup.Order = repairGroups.Count();
                }
            }
            else if (input.Order - repairGroup.Order <= -1)
            {
                var repairGroupsAstrict = _repository.WithDetails().Where(x => x.ParentId == input.ParentId && x.Order >= input.Order && x.Order < repairGroup.Order);

                foreach (var repairsAstrict in repairGroupsAstrict)
                {
                    repairsAstrict.Order += 1;
                }
                repairGroup.Order = input.Order;
                /*输入的序号可以为0时
                 * if (input.Order >= 1)
                {
                    repairGroup.Order = input.Order;
                }
                else
                {
                    repairGroup.Order = 1;
                }
                foreach (var repairsAstrict in repairGroupsAstrict)
                {
                    repairsAstrict.Order += 1;
                }*/
            }
            repairGroup.Name = input.Name;
            repairGroup.Remark = input.Remark;
            var result = _repository.UpdateAsync(repairGroup);
            if (result.Result == null)
            {
                return false;
            }
            return true;
        }

        [Authorize(StdBasicPermissions.RepairGroup.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var repairGroup = await _repository.GetAsync(id);
            if (repairGroup == null) throw new UserFriendlyException("该分组不存在");
            if (repairGroup.Children != null && repairGroup.Children.Count() > 0) throw new UserFriendlyException("需要先删除下级分类");
            await _repository.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// 查询相同名称
        /// </summary>
        private bool saveCheckSameName(string name, Guid? id, Guid? parentId)
        {
            var sameRepairGroup = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());   //将名字相同的选出
            if (parentId != null && parentId != Guid.Empty)
            {
                sameRepairGroup = sameRepairGroup.Where(x => x.ParentId == parentId);   //将同一父级下的名字相同的选出
            }
            else
            {
                sameRepairGroup = sameRepairGroup.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);   //将同一父级下的名字相同的选出
            }
            if (id.HasValue)
            {
                sameRepairGroup = sameRepairGroup.Where(o => o.Id != id.Value && o.ParentId == parentId);   //将同一父级下、除自己之外的的选出
            }
            if (sameRepairGroup.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前分组中存在相同名称的分组"); //如果有，则提示
            }
            return true;
        }

      
    }
}
