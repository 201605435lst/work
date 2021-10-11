using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentRltFileCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public List<Guid> FileIds { get; set; }
    }
}
