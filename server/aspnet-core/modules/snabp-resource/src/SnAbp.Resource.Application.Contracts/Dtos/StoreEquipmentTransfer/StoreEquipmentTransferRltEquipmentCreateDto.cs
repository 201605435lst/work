using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Dtos
{
   public class StoreEquipmentTransferRltEquipmentCreateDto : Entity<Guid>
    {
        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        /// <summary>
        /// 设备出入库记录单
        /// </summary>
        public Guid StoreEquipmentTransferId { get; set; }
    }
}
