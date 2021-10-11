using SnAbp.Emerg.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Emerg.Dtos
{
    public class ApplyEmergPlanDto
    {
        /// <summary>
        /// 调用的故障预案Id
        /// </summary>
        public Guid EmergPlanId { get; set; }

        /// <summary>
        /// 故障Id
        /// </summary>
        public Guid FaultId { get; set; }
    }
}