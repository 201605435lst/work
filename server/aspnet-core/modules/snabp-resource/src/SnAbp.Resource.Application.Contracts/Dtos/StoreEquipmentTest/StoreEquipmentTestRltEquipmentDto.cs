using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentTestRltEquipmentDto : EntityDto<Guid>
    {
        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public StoreEquipmentDto StoreEquipment { get; set; }

        /// <summary>
        /// 库存设备检测单
        /// </summary>
        public Guid StoreEquipmentTestId { get; set; }
        public StoreEquipmentTestDto StoreEquipmentTest { get; set; }
    }
}
