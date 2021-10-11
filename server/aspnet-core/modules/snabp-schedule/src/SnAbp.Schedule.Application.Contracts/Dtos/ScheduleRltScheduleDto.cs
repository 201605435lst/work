using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleRltScheduleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 前置计划
        /// </summary>
        public virtual Guid FrontScheduleId { get; set; }
        public ScheduleDto FrontSchedule { get; set; }
    }
}