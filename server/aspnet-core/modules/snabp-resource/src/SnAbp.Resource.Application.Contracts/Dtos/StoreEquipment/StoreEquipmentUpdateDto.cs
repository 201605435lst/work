using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Dtos
{
    public class StoreEquipmentUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public StoreEquipmentState State { get; set; }
    }
}
