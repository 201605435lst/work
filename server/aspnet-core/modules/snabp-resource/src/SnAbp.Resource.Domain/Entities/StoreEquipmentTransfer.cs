using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备出入库记录单
    /// </summary>
    public class StoreEquipmentTransfer : FullAuditedEntity<Guid>
    {
        protected StoreEquipmentTransfer() { }
        public StoreEquipmentTransfer(Guid id) { Id = id; }

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }

        /// <summary>
        /// 仓库 Id
        /// </summary>
        public Guid StoreHouseId { get; set; }
        public StoreHouse StoreHouse { get; set; }

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
        public List<StoreEquipmentTransferRltEquipment> StoreEquipmentTransferRltEquipments { get; set; }
    }
}
