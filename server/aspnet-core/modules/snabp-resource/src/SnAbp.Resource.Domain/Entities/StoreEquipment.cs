using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 库存设备
    /// </summary>
    public class StoreEquipment : FullAuditedEntity<Guid>
    {
        protected StoreEquipment() { }
        public StoreEquipment(Guid id) { Id = id; }

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }

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
        public ComponentCategory ComponentCategory { get; set; }


        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 厂家
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public StoreEquipmentState State { get; set; } = StoreEquipmentState.UnActived;

        /// <summary>
        /// 仓库
        /// </summary>
        public Guid? StoreHouseId { get; set; }
        public StoreHouse StoreHouse { get; set; }
        /// <summary>
        /// 关联设备
        /// </summary>
        public List<StoreEquipmentTransferRltEquipment> StoreEquipmentTransferRltEquipments { get; set; }

        public List<StoreEquipmentTestRltEquipment> StoreEquipmentTestRltEquipments { get; set; }
        /// <summary>
        /// 关联设备上下道
        /// </summary>
        public List<EquipmentServiceRecord> EquipmentServiceRecords { get; set; }
    }
}
