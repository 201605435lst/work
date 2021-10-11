using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Dtos
{
    public class FaultRltEquipmentCreateDto:EntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid Id { get; set; }
    }
}