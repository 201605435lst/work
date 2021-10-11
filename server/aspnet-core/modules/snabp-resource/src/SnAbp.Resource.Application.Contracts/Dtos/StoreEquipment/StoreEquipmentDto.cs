

using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentDto : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 库存编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }
        public IdentityUser Creator { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public StoreEquipmentComponentCategoryDto ComponentCategory { get; set; }


        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        public StoreEquipmentProductCategoryDto ProductCategory { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public Guid ManufacturerId { get; set; }
        public StoreEquipmentManufacturerDto Manufacturer { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public StoreEquipmentState State { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }
        public StoreHouseSimpleDto StoreHouse { get; set; }
        /// <summary>
        /// 设备出入库记录单
        /// </summary>
        public Guid StoreEquipmentTransferId { get; set; }

        public List<StoreEquipmentTransfer> StoreEquipmentTransfer { get; set; }
        /// <summary>
        /// 库存设备检测单
        /// </summary>
        public Guid StoreEquipmentTestId { get; set; }
        public List<StoreEquipmentTest> StoreEquipmentTest { get; set; }
        /// <summary>
        /// 关联设备
        /// </summary>
        public List<StoreEquipmentTestRltEquipmentDto> StoreEquipmentTestRltEquipments { get; set; }
        /// <summary>
        /// 关联设备出入库
        /// </summary>
        public List<StoreEquipmentTransferRltEquipmentDto> StoreEquipmentTransferRltEquipments { get; set; }
        /// <summary>
        /// 关联设备上下道
        /// </summary>
        public List<EquipmentServiceRecordMiniDto> EquipmentServiceRecords { get; set; }

    }
}
