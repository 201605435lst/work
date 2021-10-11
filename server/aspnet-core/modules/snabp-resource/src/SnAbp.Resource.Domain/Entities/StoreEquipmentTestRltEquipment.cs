using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 库存设备检测单与设备关联表
    /// </summary>
    public class StoreEquipmentTestRltEquipment : Entity<Guid>
    {
        protected StoreEquipmentTestRltEquipment() { }
        public StoreEquipmentTestRltEquipment(Guid id) { Id = id; }

        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public StoreEquipment StoreEquipment { get; set; }

        /// <summary>
        /// 库存设备检测单
        /// </summary>
        public Guid StoreEquipmentTestId { get; set; }
        public StoreEquipmentTest StoreEquipmentTest { get; set; }
    }
}
