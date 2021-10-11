using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentTransferDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 仓库 Id
        /// </summary>
        public Guid StoreHouseId { get; set; }
        public StoreHouseDto StoreHouse { get; set; }

        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid? UserId { get; set; }
        public IdentityUser User { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public StoreEquipmentTransferType Type { get; set; }

        /// <summary>
        /// 关联设备
        /// </summary>
        public List<StoreEquipmentTransferRltEquipmentDto> storeEquipmentTransferRltEquipments { get; set; } = new List<StoreEquipmentTransferRltEquipmentDto>();

    }
}
