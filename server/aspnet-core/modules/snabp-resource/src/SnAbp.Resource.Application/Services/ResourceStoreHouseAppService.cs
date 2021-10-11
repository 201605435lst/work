using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Dtos.StoreHouse;
using SnAbp.Resource.Entities;
using SnAbp.Store.IServices;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Store.Services
{
    [Authorize]
    public class ResourceStoreHouseAppService : ResourceAppService, IResourceStoreHouseAppService
    {

        private readonly IRepository<StoreHouse, Guid> _storeStoreHouseRepository;
        private readonly IRepository<StoreEquipment, Guid> _storeEquipmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private UnitOfWorkManager _unit;
        public ResourceStoreHouseAppService(
        IRepository<StoreHouse, Guid> storeStoreHouseRepository,
        IRepository<StoreEquipment, Guid> storeEquipmentRepository,
        UnitOfWorkManager unit,
            IGuidGenerator guidGenerator)
        {
            _storeStoreHouseRepository = storeStoreHouseRepository;
            _storeEquipmentRepository = storeEquipmentRepository;
            _guidGenerator = guidGenerator;
            _unit = unit;
        }
        /// <summary>
        /// 获取仓库实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StoreHouseDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var storeHouse = _storeStoreHouseRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (storeHouse == null) throw new UserFriendlyException("此仓库不存在");
            return Task.FromResult(ObjectMapper.Map<StoreHouse, StoreHouseDto>(storeHouse));
        }
        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreHouseDto>> GetList(StoreHouseSearchDto input)
        {
            var result = new PagedResultDto<StoreHouseDto>();
            var storeHouse = new List<StoreHouse>();
            var dto = new List<StoreHouseDto>();
            if (input.Ids != null && input.Ids.Count > 0)
            {
                foreach (var id in input.Ids)
                {
                    //判断是否仓库选择树
                    if (input.Status)
                    {
                        var storeHouses = _storeStoreHouseRepository.WithDetails().FirstOrDefault(x => x.Id == id && x.Enabled == true);
                        if (storeHouses == null) continue;
                        var filterList = _storeStoreHouseRepository
                            .WithDetails()
                            .Where(x => x.ParentId == storeHouses.ParentId || x.ParentId == null)
                            .ToList();
                        storeHouse.AddRange(filterList);

                    }
                    else
                    {
                        var storeHouses = _storeStoreHouseRepository.WithDetails().FirstOrDefault(x => x.Id == id);
                        if (storeHouses == null) continue;
                        var filterList = _storeStoreHouseRepository
                            .WithDetails()
                            .Where(x => x.ParentId == storeHouses.ParentId || x.ParentId == null)
                            .ToList();
                        storeHouse.AddRange(filterList);
                    }

                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<StoreHouse>, List<StoreHouseDto>>(storeHouse.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (input.Status)
                    {
                        if (item.Children != null && item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            if (item.Children != null && item.Children.Count > 0)
                            {
                                var childrenData = item.Children;
                                var childrenStoreHouse = childrenData.Where(x => x.Enabled == true).FirstOrDefault();
                                item.Children = childrenStoreHouse != null ? new List<StoreHouseDto>() : null;
                            }
                        }
                    }
                    else
                    {
                        item.Children = item.Children.Count == 0 ? null : new List<StoreHouseDto>();
                    }
                }

                dto = GuidKeyTreeHelper<StoreHouseDto>.GetTree(listDtos);
            }
            else
            {
                //判断是否仓库选择树
                if (input.Status)
                {
                    storeHouse = _storeStoreHouseRepository.WithDetails()
                                                  .Where(x => x.Enabled == true && x.ParentId == input.ParentId).ToList();
                    dto = ObjectMapper.Map<List<StoreHouse>, List<StoreHouseDto>>(storeHouse);
                    foreach (var item in dto)
                    {
                        if (item.Children != null && item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            if (item.Children != null && item.Children.Count > 0)
                            {
                                var childrenData = item.Children;
                                var childrenStoreHouse = childrenData.Where(x => x.Enabled == true).FirstOrDefault();
                                item.Children = childrenStoreHouse != null ? new List<StoreHouseDto>() : null;
                            }
                        }
                    }
                }
                else
                {
                    storeHouse = _storeStoreHouseRepository.WithDetails()
                               .WhereIf((input.OrganizationId == null || input.OrganizationId == Guid.Empty) &&
                               !input.Enabled.HasValue &&
                               !input.AreaId.HasValue &&
                               string.IsNullOrEmpty(input.KeyWords), x => x.ParentId == input.ParentId)
                               .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.OrganizationId == input.OrganizationId)
                               .WhereIf(input.Enabled.HasValue, x => x.Enabled == input.Enabled)
                               .WhereIf(input.AreaId.HasValue, x => x.AreaId.ToString().StartsWith(input.AreaId.ToString()))
                               .WhereIf(!string.IsNullOrEmpty(input.KeyWords),
                                                x => x.Name.Contains(input.KeyWords) ||
                                                x.Address.Contains(input.KeyWords)).ToList();
                    dto = ObjectMapper.Map<List<StoreHouse>, List<StoreHouseDto>>(storeHouse);
                    foreach (var item in dto)
                    {
                        item.Children = (item.Children.Count == 0 && (string.IsNullOrEmpty(input.KeyWords) &&
                                       (input.OrganizationId == null || input.OrganizationId == Guid.Empty) &&
                                        !input.Enabled.HasValue && !input.AreaId.HasValue)) || (!string.IsNullOrEmpty(input.KeyWords) ||
                                       (input.OrganizationId != null && input.OrganizationId != Guid.Empty) ||
                                        input.Enabled.HasValue || input.AreaId.HasValue) ? null : new List<StoreHouseDto>();

                    }
                }




            }
            result.TotalCount = dto.Count();
            result.Items = input.IsAll ? dto :
                dto.OrderBy(x => x.Order).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }
        private List<StoreHouse> GetChildren(IEnumerable<StoreHouse> data, Guid? Id)
        {
            List<StoreHouse> list = new List<StoreHouse>();
            var children = data.Where(p => p.ParentId == Id);
            foreach (var item in children)
            {
                var node = new StoreHouse(item.Id);
                node.Address = item.Address;
                node.AreaId = item.AreaId;
                node.Enabled = item.Enabled;
                node.Name = item.Name;
                node.Order = item.Order;
                node.Area = item.Area;
                node.Position = item.Position;
                node.ParentId = item.ParentId;
                node.OrganizationId = item.OrganizationId;
                node.Children = GetChildren(data, node.Id);
                list.Add(node);
            }
            return list;

        }

        private List<StoreHouse> GetChildrenKeyWord(IEnumerable<StoreHouse> data, StoreHouseSearchDto input)
        {
            List<StoreHouse> list = new List<StoreHouse>();
            foreach (var item in data)
            {
                var node = new StoreHouse(item.Id);
                node.Address = item.Address;
                node.AreaId = item.AreaId;
                node.Enabled = item.Enabled;
                node.Name = item.Name;
                node.Order = item.Order;
                node.Area = item.Area;
                node.Position = item.Position;
                node.ParentId = item.ParentId;
                node.OrganizationId = item.OrganizationId;
                list.Add(node);
            }
            return list;
        }
        /// <summary>
        /// 创建仓库实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreHouse.Create)]
        public async Task<StoreHouseDto> Create(StoreHouseCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("仓库名称不能为空");
            CheckSameName(input.Name, null, input.ParentId);
            var storeHouse = new StoreHouse(_guidGenerator.Create());
            storeHouse.Name = input.Name;

            storeHouse.Enabled = input.Enabled;
            storeHouse.Address = input.Address;
            storeHouse.ParentId = input.ParentId;
            storeHouse.Position = input.Position;
            storeHouse.Order = input.Order;
            storeHouse.AreaId = input.AreaId;
            storeHouse.OrganizationId = input.OrganizationId;
            await _storeStoreHouseRepository.InsertAsync(storeHouse);

            return ObjectMapper.Map<StoreHouse, StoreHouseDto>(storeHouse);

        }
        /// <summary>
        /// 修改仓库实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreHouse.Update)]
        public async Task<StoreHouseDto> Update(StoreHouseUpdateDto input)
        {

            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入仓库的id");
            var storeHouse = await _storeStoreHouseRepository.GetAsync(input.Id);
            if (storeHouse == null) throw new UserFriendlyException("该仓库不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new Volo.Abp.UserFriendlyException("名称不能为空");
            CheckSameName(input.Name, input.Id, input.ParentId);
            storeHouse.Name = input.Name;
            storeHouse.ParentId = input.ParentId;
            storeHouse.Enabled = input.Enabled;
            storeHouse.Address = input.Address;
            storeHouse.AreaId = input.AreaId;
            storeHouse.Position = input.Position;
            storeHouse.Order = input.Order;
            storeHouse.OrganizationId = input.OrganizationId;
            await _storeStoreHouseRepository.UpdateAsync(storeHouse);

            return ObjectMapper.Map<StoreHouse, StoreHouseDto>(storeHouse);

        }
        private bool CheckSameName(string Name, Guid? id, Guid? parentId)
        {
            var sameStoreHouse = _storeStoreHouseRepository.Where(o => o.Name.ToUpper() == Name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                sameStoreHouse = sameStoreHouse.Where(x => x.ParentId == parentId);
            }
            else
            {
                sameStoreHouse = sameStoreHouse.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameStoreHouse = sameStoreHouse.Where(x => x.Id != id.Value);
            }
            if (sameStoreHouse.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前仓库已存在相同名字的仓库！！！");
            }
            return true;
        }
        /// <summary>
        /// 修改是否启用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreHouse.UpdateEnable)]
        public async Task<StoreHouseDto> UpdateEnable(StoreHouseUpdateEnableDto input)
        {
            var storeHouse = await _storeStoreHouseRepository.GetAsync(input.Id);
            if (storeHouse == null) throw new UserFriendlyException("该仓库不存在");
            storeHouse.Enabled = input.Enabled;
            await _storeStoreHouseRepository.UpdateAsync(storeHouse);
            return ObjectMapper.Map<StoreHouse, StoreHouseDto>(storeHouse);
        }
        /// <summary>
        /// 删除仓库实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreHouse.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var storeHouse = _storeStoreHouseRepository.WithDetails(x => x.Children).Where(x => x.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(storeHouse.Name)) throw new UserFriendlyException("该仓库不存在");
            if (storeHouse.Children != null && storeHouse.Children.Count > 0) throw new UserFriendlyException("请先删除该产品的下级分类");
            var storeEquipment = _storeEquipmentRepository.WithDetails(x => x.StoreHouse).Where(x => x.StoreHouseId == id).FirstOrDefault();
            if (storeEquipment == null)
            {
                await _storeStoreHouseRepository.DeleteAsync(id);
            }
            else
            {
                throw new UserFriendlyException("该仓库已经被录入设备，不能删除");
            }

            return true;

        }


    }
}
