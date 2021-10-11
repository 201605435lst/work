using SnAbp.Basic.Entities;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices.Equipment;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using SnAbp.Resource.Enums;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Authorization;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceEquipmentPropertyAppService : ResourceAppService, IResourceEquipmentPropertyAppService
    {
        private readonly IRepository<Equipment, Guid> equipmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<EquipmentProperty, Guid> equipmentProperty;
        private readonly IRepository<Model, Guid> modelRepository;//标准库标准设备

        public ResourceEquipmentPropertyAppService(
            IRepository<Equipment, Guid> equipmentRep,
            IRepository<EquipmentProperty, Guid> equipmentProtertyRep,
            IRepository<Model, Guid> modelRep,
            IGuidGenerator guidGenerator
            )
        {
            equipmentRepository = equipmentRep;
            equipmentProperty = equipmentProtertyRep;
            modelRepository = modelRep;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public Task<List<EquipmentPropertyDto>> GetList(Guid equipmentId)
        {
            var equipment = equipmentRepository
                .WithDetails(
                    x => x.ComponentCategory,
                    x => x.Organization,
                    x => x.InstallationSite,
                    x => x.InstallationSite.Railway,
                    x => x.InstallationSite.Station,
                    x => x.Group,
                    x => x.CableExtend
                )
                .FirstOrDefault(x => x.Id == equipmentId);

            if (equipment == null) throw new UserFriendlyException("设备实体不存在");

            var standardEquipment = modelRepository
                .WithDetails(x => x.Manufacturer)
                .FirstOrDefault(x =>
                    x.ProductCategoryId == equipment.ProductCategoryId &&
                    x.ManufacturerId == equipment.ManufacturerId
                );

            // 查询基本属性
            var dtoList = new List<EquipmentPropertyDto>()
            {
                new EquipmentPropertyDto() {
                    Name = "设备名称",
                    Value = equipment.Name ,
                    Order = 1 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "设备分组",
                    Value = equipment.Group.Name ,
                    Order = 1 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "设备类型",
                    Value = equipment.ComponentCategory != null ? equipment.ComponentCategory.Name: "",
                    Order = 2,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "所属线路",
                    Value = equipment.InstallationSite.Railway != null ? equipment.InstallationSite.Railway.Name : "" ,
                    Order = 3 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "所在站点",
                    Value = equipment.InstallationSite.Station != null ? equipment.InstallationSite.Station.Name : "" ,
                    Order = 4 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "安装位置",
                    Value = equipment.InstallationSite != null ? equipment.InstallationSite.Name : "" ,
                    Order = 5 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "所属厂家",
                    Value = standardEquipment !=null && standardEquipment.Manufacturer !=null ? standardEquipment.Manufacturer.Name : "" ,
                    Order = 6 ,
                    Type = EquipmentPropertyType.Default
                },
                new EquipmentPropertyDto() {
                    Name = "使用寿命",
                    Value = standardEquipment !=null ? standardEquipment.ServiceLife.ToString() + standardEquipment.ServiceLifeUnit: "" ,
                    Order = 8 ,
                    Type = EquipmentPropertyType.Default
                },

            };


            // 查询扩展属性
            var extendProperties = equipmentProperty
                .WithDetails(x => x.MVDProperty.MVDCategory)
                .Where(x => x.EquipmentId == equipment.Id)
                .OrderBy(x => x.MVDProperty.MVDCategory.Order)
                .ThenBy(x => x.MVDProperty.MVDCategory.Name)
                .ThenBy(x => x.MVDProperty.Order)
                .ThenBy(x => x.MVDProperty.Name)
                .ToList();

            var extendPropertiesDto = ObjectMapper.Map<List<EquipmentProperty>, List<EquipmentPropertyDto>>(extendProperties);

            foreach (var item in extendPropertiesDto)
            {
                dtoList.Add(new EquipmentPropertyDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Value = item.Value,
                    Order = item.Order,
                    Type = EquipmentPropertyType.Extend,
                    MVDCategoryId = item.MVDCategoryId,
                    MVDCategory = item.MVDCategory,
                    MVDPropertyId = item.MVDPropertyId,
                    MVDProperty = item.MVDProperty,
                });
            }

            //添加电缆属性
            if (equipment.CableExtend != null)
            {
                var cableProperty = new List<EquipmentPropertyDto>()
                {
                   new EquipmentPropertyDto() {
                       Id = equipment.CableExtend.Id,
                       Name = "芯数",
                       Value = equipment.CableExtend.Number.HasValue  ? equipment.CableExtend.Number.ToString() : "",
                       Type = EquipmentPropertyType.CableProperty
                   },
                   new EquipmentPropertyDto() {
                       Id = equipment.CableExtend.Id,
                       Name = "备用芯数",
                       Value = equipment.CableExtend.SpareNumber.HasValue  ? equipment.CableExtend.SpareNumber.ToString() : "",
                       Type = EquipmentPropertyType.CableProperty
                   },
                   new EquipmentPropertyDto() {
                       Id = equipment.CableExtend.Id,
                       Name = "路产芯数",
                       Value = equipment.CableExtend.RailwayNumber.HasValue  ? equipment.CableExtend.RailwayNumber.ToString() : "",
                       Type = EquipmentPropertyType.CableProperty
                   },
                   new EquipmentPropertyDto() {
                       Id = equipment.CableExtend.Id,
                       Name = "皮长公里",
                       Value = equipment.CableExtend.Length.HasValue ? equipment.CableExtend.Length.ToString() : "",
                       Type = EquipmentPropertyType.CableProperty
                   },
                   new EquipmentPropertyDto() {
                       Id = equipment.CableExtend.Id,
                       Name = "铺设类型",
                       Value = equipment.CableExtend.LayType.HasValue? ((int)equipment.CableExtend.LayType).ToString() : null,
                       Type = EquipmentPropertyType.CableProperty
                   },
                };
                dtoList.AddRange(cableProperty);
            }
            return Task.FromResult(dtoList);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.EquipmentProperty.Create)]
        public async Task<bool> Create(EquipmentPropertyCreateDto input)
        {
            var equipment = equipmentRepository.FirstOrDefault(x => x.Id == input.EquipmentId);

            if (equipment == null) throw new UserFriendlyException("该设备实体不存在");

            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备属性名称不能为空");

            if (string.IsNullOrEmpty(input.Value)) throw new UserFriendlyException("设备属性值不能为空");

            var sameEntity = equipmentProperty.FirstOrDefault(x => x.Name == input.Name && x.EquipmentId == input.EquipmentId);
            if (sameEntity != null) throw new UserFriendlyException("该设备属性名称已存在");

            var property = new EquipmentProperty(_guidGenerator.Create())
            {
                EquipmentId = input.EquipmentId,
                Name = input.Name,
                Value = input.Value,
                Order = input.Order,
            };
            await equipmentProperty.InsertAsync(property);
            return true;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.EquipmentProperty.Update)]
        public async Task<bool> Update(EquipmentPropertyUpdateDto input)
        {
            var equipment = equipmentRepository.FirstOrDefault(x => x.Id == input.EquipmentId);

            if (equipment == null) throw new UserFriendlyException("该设备实体不存在");

            var property = equipmentProperty.FirstOrDefault(x => x.Id == input.Id);

            if (property == null) throw new UserFriendlyException("该设备属性不存在");

            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("设备属性名称不能为空");

            if (string.IsNullOrEmpty(input.Value)) throw new UserFriendlyException("设备属性值不能为空");

            var sameEntity = equipmentProperty.FirstOrDefault(x => x.Name == input.Name && x.EquipmentId == input.EquipmentId && x.Id != input.Id);
            if (sameEntity != null) throw new UserFriendlyException("该设备属性名称已存在");

            property.EquipmentId = input.EquipmentId;
            property.Name = input.Name;
            property.Value = input.Value;
            property.Order = input.Order;
            await equipmentProperty.UpdateAsync(property);
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.EquipmentProperty.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                if (id == null || Guid.Empty == id) throw new Exception("id不正确");
                var ent = equipmentProperty.FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new Exception("该属性不存在");
                await equipmentProperty.DeleteAsync(id);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

    }
}