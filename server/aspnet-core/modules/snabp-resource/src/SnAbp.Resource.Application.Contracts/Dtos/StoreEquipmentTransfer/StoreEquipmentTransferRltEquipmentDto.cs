using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource
{
    public class StoreEquipmentTransferRltEquipmentDto : EntityDto<Guid>
    {
        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        public  StoreEquipmentDto StoreEquipment { get; set; }

        /// <summary>
        /// 设备出入库记录单
        /// </summary>
        public Guid StoreEquipmentTransferId { get; set; }
        public StoreEquipmentTransferDto StoreEquipmentTransfer { get; set; }
    }
}