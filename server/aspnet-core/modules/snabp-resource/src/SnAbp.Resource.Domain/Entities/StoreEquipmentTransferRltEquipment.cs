using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备出入库记录单与设备关联表
    /// </summary>
    public class StoreEquipmentTransferRltEquipment : Entity<Guid>
    {
        protected StoreEquipmentTransferRltEquipment() { }
        public StoreEquipmentTransferRltEquipment(Guid id) { Id = id; }

        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public StoreEquipment StoreEquipment { get; set; }

        /// <summary>
        /// 设备出入库记录单
        /// </summary>
        public Guid StoreEquipmentTransferId { get; set; }
        public StoreEquipmentTransfer StoreEquipmentTransfer { get; set; }
    }
}
