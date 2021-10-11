using SnAbp.Resource.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class FaultRltEquipmentSimpleDto:EntityDto<Guid>
    {

        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }
    }
}