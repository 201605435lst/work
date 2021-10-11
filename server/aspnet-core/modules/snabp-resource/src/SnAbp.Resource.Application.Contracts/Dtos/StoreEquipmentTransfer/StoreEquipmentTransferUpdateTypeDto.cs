using SnAbp.Resource.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
   public class StoreEquipmentTransferUpdateTypeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public StoreEquipmentTransferType Type { get; set; }
    }
}
